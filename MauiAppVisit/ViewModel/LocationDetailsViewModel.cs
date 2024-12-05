using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiAppVisit.Helpers;
using MauiAppVisit.Model;
using MauiAppVisit.Platforms.Android;
using System.IO.Compression;
using System.Text.Json;
using System.Windows.Input;

namespace MauiAppVisit.ViewModel
{
    public partial class LocationDetailsViewModel : ObservableObject
    {
        private HttpHelper HttpHelper { get; set; }

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
                        var data = await JsonSerializer.DeserializeAsync<List<Lugar>>(responseStream);

                        Idarquivo = data[0].fileVRId;
                        DescriptionPlace = data[0].description;
                        Nome = data[0].name;
                        ImagePlaceByte = Convert.FromBase64String(data[0].image);
                        IdLugarInfo = IdLugar.ToString();
                        NameButton = AndroidUtils.VerifyAppInstaledButton(data[0].fileName) ? "INICIAR" : "BAIXAR";
                    }
                    Loading = "false";
                }
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
            Loading = "true";
            Aviso = "";
            var baseUrl = HttpHelper.GetBaseUrl();
            var httpClient = await HttpHelper.GetHttpClient();
            bool hasInstalled = true;

            try
            {
                var url = $"{baseUrl}/FileVR/dadosArquivos/{Idarquivo}";
                var responseDetailsFileVR = await httpClient.GetAsync(url);

                if (responseDetailsFileVR.IsSuccessStatusCode)
                {
                    var responseContentFileVrDetailsStream = await responseDetailsFileVR.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<List<FileVrDetails>>(responseContentFileVrDetailsStream);

                    hasInstalled = AndroidUtils.VerifyAppInstaled($"{data[0].fileName}.apk");

                    if (hasInstalled)
                    {
                        Loading = "false";
                        return;
                    }
                    
                }

                if (!hasInstalled)
                {
                    url = $"{baseUrl}/FileVR/{Idarquivo}";
                    var responseFile = await httpClient.GetAsync(url);

                    if (responseFile.IsSuccessStatusCode)
                    {
                        var responseContent = await responseFile.Content.ReadAsStreamAsync();
                        using (var arquivos = new ZipArchive(responseContent, ZipArchiveMode.Read))
                        {
                            AndroidUtils.GrantedPermission();

                            var arquivoApk = arquivos.Entries[0];
                            var streamAPK = arquivoApk.Open();

                            Loading = "false";

                            await AndroidUtils.VerifyAppDownload(streamAPK, arquivoApk.Name);
                        }
                    }
                }
            }
            catch(Exception)
            {
                Loading = "false";
                Aviso = "Servidor indisponível, por favor tente novamente mais tarde!";
            }
        }
    }
}
