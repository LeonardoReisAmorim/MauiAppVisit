using CommunityToolkit.Mvvm.ComponentModel;
using MauiAppVisit.Model;
using MauiAppVisit.WebServiceHttpClient;
#if ANDROID
using MauiAppVisit.Platforms.Android;
#endif
using System.IO.Compression;
using System.Windows.Input;

namespace MauiAppVisit.ViewModel
{
    public partial class LocationDetailsViewModel : ObservableObject
    {
        private readonly IWebService _webService;

        private int Idarquivo;

        [ObservableProperty]
        private string _descriptionPlace;

        [ObservableProperty]
        private string _nameButton;

        [ObservableProperty]
        private string _nome;

        [ObservableProperty]
        private string _loading;

        [ObservableProperty]
        private string _aviso;

        [ObservableProperty]
        private byte[] _imagePlaceByte;

        [ObservableProperty]
        private string _idLugarInfo;

        public LocationDetailsViewModel(IWebService webService)
        {
            _webService = webService;
            Loading = "true";
            Aviso = "";
        }

        public async Task GetLocationDetailsById(int id)
        {
            try
            {
                var data = await _webService.GetLocationDetailsById(id);
                Idarquivo = data[0].FileVRId;
                DescriptionPlace = data[0].Description;
                Nome = data[0].Name;
                ImagePlaceByte = Convert.FromBase64String(data[0].Image);
                IdLugarInfo = id.ToString();
#if ANDROID
                        NameButton = AndroidUtils.HasAppInstalledButton(data[0].FileName) ? "INICIAR" : "BAIXAR";
#endif
                Loading = "false";
            }
            catch (Exception)
            {
                Loading = "false";
                Aviso = "Servidor indisponível, por favor tente novamente mais tarde!";
            }
        }

        public ICommand GetArquivoCommand => new Command(async () => await GetArquivoAsync());

        private async Task GetArquivoAsync()
        {
            try
            {
                FileVrDetails fileVrDetails = await RequestFileVrDetails();

#if ANDROID
                bool processFileVR = AndroidUtils.ProcessFileVR(fileVrDetails);

                if (!processFileVR)
                {
                    await RequestDownloadFileVR(fileVrDetails);
                    return;
                }
#endif

                Loading = "false";
            }
            catch (Exception)
            {
                Loading = "false";
                Aviso = "Servidor indisponível, por favor tente novamente mais tarde!";
            }
        }

        private async Task RequestDownloadFileVR(FileVrDetails fileVrDetails)
        {
            try
            {
                var responseContent = await _webService.RequestDownloadFileVR(Idarquivo);
                using (var arquivos = new ZipArchive(responseContent, ZipArchiveMode.Read))
                {
#if ANDROID
                    AndroidUtils.GrantedPermission();
#endif

                    var arquivoApk = arquivos.Entries[0];
                    var streamAPK = arquivoApk.Open();

                    Loading = "false";

#if ANDROID
                    await AndroidUtils.DownloadApk(streamAPK, arquivoApk.Name.ToLower(), fileVrDetails);
#endif
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<FileVrDetails> RequestFileVrDetails()
        {
            try
            {
                return await _webService.RequestFileVrDetails(Idarquivo);
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
