using CommunityToolkit.Mvvm.ComponentModel;
using MauiAppVisit.Helpers;
using MauiAppVisit.Model;
#if ANDROID
using MauiAppVisit.Platforms.Android;
#endif
using System.IO.Compression;
using System.Text.Json;
using System.Windows.Input;

namespace MauiAppVisit.ViewModel
{
    public partial class LocationDetailsViewModel : ObservableObject
    {
        private HttpHelper HttpHelper { get; set; }
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        private readonly int IdLugar;
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

        public LocationDetailsViewModel(string Id)
        {
            IdLugar = Convert.ToInt32(Id);
            HttpHelper = new HttpHelper();
            Loading = "true";
            Aviso = "";
            _jsonSerializerOptions = JsonSerializeOptionHelper.Options;
        }

        public async Task GetLocationDetailsById()
        {
            var baseUrl = HttpHelper.GetBaseUrl();
            var htppClient = await HttpHelper.GetHttpClient();

            var url = $"{baseUrl}/Place/{IdLugar}";

            try
            {
                var response = await htppClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    using (var responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        var data = await JsonSerializer.DeserializeAsync<List<Lugar>>(responseStream, _jsonSerializerOptions);

                        Idarquivo = data[0].FileVRId;
                        DescriptionPlace = data[0].Description;
                        Nome = data[0].Name;
                        ImagePlaceByte = Convert.FromBase64String(data[0].Image);
                        IdLugarInfo = IdLugar.ToString();
#if ANDROID
                        //NameButton = AndroidUtils.HasAppInstalledButton(data[0].FileName) ? "INICIAR" : "BAIXAR";
#endif
                    }
                    Loading = "false";
                }
            }
            catch (Exception ex)
            {
                Loading = "false";
                Aviso = "Servidor indisponível, por favor tente novamente mais tarde!";
            }
        }

        public ICommand GetArquivoCommand => new Command(async () => await GetArquivoAsync());

        private async Task GetArquivoAsync()
        {
            Loading = "true";
            Aviso = "";
            var baseUrl = HttpHelper.GetBaseUrl();
            var httpClient = await HttpHelper.GetHttpClient();

            try
            {
                FileVrDetails fileVrDetails = await RequestFileVrDetails(baseUrl, httpClient);

                #if ANDROID
                bool processFileVR = AndroidUtils.ProcessFileVR(fileVrDetails);

                if (!processFileVR)
                {
                    await RequestDownloadFileVR(baseUrl, httpClient, fileVrDetails);
                    return;
                }
                #endif

                Loading = "false";
            }
            catch(Exception ex)
            {
                Loading = "false";
                Aviso = "Servidor indisponível, por favor tente novamente mais tarde!";
            }
        }

        private async Task RequestDownloadFileVR(string baseUrl, HttpClient httpClient, FileVrDetails fileVrDetails)
        {
            string url = $"{baseUrl}/FileVR/{Idarquivo}";
            var responseFile = await httpClient.GetAsync(url);

            if (responseFile.IsSuccessStatusCode)
            {
                var responseContent = await responseFile.Content.ReadAsStreamAsync();
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
        }

        private async Task<FileVrDetails> RequestFileVrDetails(string baseUrl, HttpClient httpClient)
        {
            var url = $"{baseUrl}/FileVR/dadosArquivos/{Idarquivo}";
            var responseDetailsFileVR = await httpClient.GetAsync(url);

            if (responseDetailsFileVR.IsSuccessStatusCode)
            {
                var responseContentFileVrDetailsStream = await responseDetailsFileVR.Content.ReadAsStringAsync();
                var fileVrDetailsList = JsonSerializer.Deserialize<List<FileVrDetails>>(responseContentFileVrDetailsStream, _jsonSerializerOptions);
                return fileVrDetailsList[0];
            }

            return new FileVrDetails();
        }
    }
}
