using CommunityToolkit.Maui;
using FinanceApp.Services;
using FinanceApp.Services.Charting;
using FinanceApp.ViewModels;
using FinanceApp.Views;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace FinanceApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
				.UseSkiaSharp()
				.ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("materialdesignicons-webfont.ttf", "MaterialDesignIcons");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
			builder.Services.AddSingleton<IPricePathSimulator, BrownianSimulator>();
			builder.Services.AddSingleton<IPriceChartRenderer, PriceChartRenderer>();

			builder.Services.AddTransient<HomePageViewModel>();
            builder.Services.AddTransient<HomePage>();

            return builder.Build();
        }
    }
}
