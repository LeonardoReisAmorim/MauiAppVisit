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
            GetLocationDetailsById();
        }

        private async void GetLocationDetailsById()
        {
            var baseUrl = HttpHelper.GetBaseUrl();
            var htppClient = HttpHelper.GetHttpClient();

            var url = $"{baseUrl}/Lugar/{IdLugar}";

            try
            {
                var response = await htppClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    using (var responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        var data = await JsonSerializer.DeserializeAsync<List<Lugar>>(responseStream);

                        Idarquivo = data[0].arquivoId;
                        DescriptionPlace = data[0].descricao;
                        Nome = data[0].nome;
                        ImagePlaceByte = Convert.FromBase64String(data[0].imagem);
                        IdLugarInfo = IdLugar.ToString();
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
            var httpClient = HttpHelper.GetHttpClient();

            var url = $"{baseUrl}/Arquivo/{Idarquivo}";

            try
            {
                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStreamAsync();
                    using (var arquivos = new ZipArchive(responseContent, ZipArchiveMode.Read))
                    {
                        AndroidUtils.GrantedPermission();

                        var arquivoApk = arquivos.Entries[0];
                        var streamAPK = arquivoApk.Open();

                        await FileSaver.Default.SaveAsync(arquivoApk.Name, streamAPK, new CancellationToken());

                        Loading = "false";
                        Aviso = $"1 - Faça o download do ambiente virtual em formato '.apk':\n2 - Instale o arquivo '{arquivoApk.Name}' referente ao ambiente virtual no diretório baixado;\n3 - Divirta-se!";
                    }
                }
            }
            catch (Exception ex)
            {
                Loading = "false";
                Aviso = "Servidor indisponível, por favor tente novamente mais tarde!";
            }
        }
    }
}
