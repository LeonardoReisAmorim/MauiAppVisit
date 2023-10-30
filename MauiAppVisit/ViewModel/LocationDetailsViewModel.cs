#if ANDROID
using Android;
using Android.Content.PM;
using AndroidX.Core.App;
using AndroidX.Core.Content;
#endif
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiAppVisit.Helpers;
using MauiAppVisit.Model;
using System.IO.Compression;
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
            Loading = "true";
            var arquivoBytesZip = Array.Empty<byte>();
            var arquivoBytesApk = Array.Empty<byte>();
            var arquivoZip = new ArquivoDTO();
            var filenameApk = string.Empty;
            var baseUrl = HttpHelper.GetBaseUrl();
            var htppClient = HttpHelper.GetHttpClient();

            var url = $"{baseUrl}/Arquivo/{Idarquivo}";
            var response = await htppClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                arquivoZip = JsonSerializer.Deserialize<ArquivoDTO>(responseContent);
            }
            arquivoBytesZip = Convert.FromBase64String(arquivoZip.arquivo);

            //using (MemoryStream msZip = new MemoryStream(arquivoBytesZip))
            //{
            //    using (var arquivos = new ZipArchive(msZip, ZipArchiveMode.Read))
            //    {
            //        foreach (ZipArchiveEntry arquivoApk in arquivos.Entries)
            //        {
            //            filenameApk = arquivoApk.Name;
            //            arquivoBytesApk = new byte[arquivoApk.Length];
            //        }
            //    }
            //}

            // this will run for Android 33 and greater
            if (DeviceInfo.Platform == DevicePlatform.Android && OperatingSystem.IsAndroidVersionAtLeast(33))
            {
            #if ANDROID
                var activity = Platform.CurrentActivity ?? throw new NullReferenceException("Current activity is null");
                if (ContextCompat.CheckSelfPermission(activity, Manifest.Permission.ReadExternalStorage) != Permission.Granted)
                {
                    ActivityCompat.RequestPermissions(activity, new[] { Manifest.Permission.ReadExternalStorage }, 1);
                }
            #endif
            }

            using var stream = new MemoryStream(arquivoBytesZip);
            await FileSaver.Default.SaveAsync("teste.zip", stream, new CancellationToken());
            Loading = "false";
        }
    }
}
