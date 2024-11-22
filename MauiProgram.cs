using Microsoft.Extensions.Logging;

namespace TiAppMovi
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiMaps()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("CreatoDisplay-Bold.otf", "Bold");
                    fonts.AddFont("CreatoDisplay-Light.otf", "Light");
                    fonts.AddFont("CreatoDisplay-Medium.otf", "Medium");
                    fonts.AddFont("CreatoDisplay-Thin.otf", "Thin");

                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
