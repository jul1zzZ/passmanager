using CommunityToolkit.Maui;
using Material.Components.Maui;
using Material.Components.Maui.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace pass
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "accounts.db3");

            builder
                .UseMauiApp<App>(app => new App(dbPath))
                 .UseMauiCommunityToolkit()
                 .UseMaterialComponents()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            return builder.Build();
        }
    }
}
