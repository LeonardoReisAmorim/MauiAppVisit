using CommunityToolkit.Mvvm.ComponentModel;
using MauiAppVisit.Helpers;
using MauiAppVisit.Model;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Text.Json;
using System.Windows.Input;

namespace MauiAppVisit.ViewModel
{
    public partial class LocationDetailsViewModel : ObservableObject
    {
        HttpHelper HttpHelper { get; set; }

        private readonly int IdLugar;
        private int Idarquivo;

        [ObservableProperty]
        private string _descriptionPlace;

        [ObservableProperty]
        private string _loading;

        [ObservableProperty]
        private byte[] _imagePlaceByte;

        public LocationDetailsViewModel(string Id)
        {
            IdLugar = Convert.ToInt32(Id);
            HttpHelper = new HttpHelper();
            Loading = "false";
            GetLocationDetailsById();
        }

        private async void GetLocationDetailsById()
        {
            var baseUrl = HttpHelper.GetBaseUrl();
            var htppClient = HttpHelper.GetHttpClient();

            var url = $"{baseUrl}/Lugar/{IdLugar}";
            var response = await htppClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var data = await JsonSerializer.DeserializeAsync<List<Lugar>>(responseStream);

                    Idarquivo = data[0].arquivoId;
                    DescriptionPlace = data[0].descricao;
                    ImagePlaceByte = Convert.FromBase64String(data[0].imagem);
                }
            }
        }

        public ICommand GetArquivoCommand => new Command(async () => await GetArquivoAsync());

        private async Task GetArquivoAsync()
        {
            //Loading = "true";
            //var baseUrl = HttpHelper.GetBaseUrl();
            //var htppClient = HttpHelper.GetHttpClient();

            //var url = $"{baseUrl}/Arquivo/{Idarquivo}";
            //var response = await htppClient.GetAsync(url);

            ////TODO - ZIPAR E DESZIPAR POR CAUSA DO TAMANHO
            //if (response.IsSuccessStatusCode)
            //{
            //    var responseContent = await response.Content.ReadAsStringAsync();
            //    var arquivo = JsonSerializer.Deserialize<ArquivoDTO>(responseContent);

            //    var arquivoBytes = Convert.FromBase64String(arquivo.arquivo);

                
            //}

            string mainDir = FileSystem.Current.AppDataDirectory;
            var fullpatch = Path.Combine(mainDir, "teste.txt");
            File.WriteAllText(fullpatch, "teste");

            Loading = "false";
        }
    }
}
