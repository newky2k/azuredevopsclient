using Microsoft.Extensions.Logging;
using System.Mvvm;

namespace SampleMAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddCoreUI();

            var app = builder.Build();

            //configure the servicehost host
            ServiceHost.Host = new MauiHostProxy(app);

           
            return app;
        }
    }
}
