using CommunityToolkit.Maui;
using MauiAppVisit.ViewModel;
using MauiAppVisit.WebServiceHttpClient;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace MauiAppVisit
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddHttpClient<IWebService, WebService>(config =>
            {
                config.BaseAddress = new Uri("https://nn7tgxqn.tunnelite.com");
                config.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

            });
            builder.Services.AddTransient<UsuarioViewModel>();
            builder.Services.AddTransient<InformationPlaceVR>();
            builder.Services.AddTransient<LocalItensViewModel>();
            builder.Services.AddTransient<LocationDetailsViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}