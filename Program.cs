using AnkiClone.Logic;
using AnkiClone.ViewModels;
using Avalonia;
using System;

namespace AnkiClone;

class Program
{
    public static Config CurrentConfig { get; set; }


    [STAThread]
    public static void Main(string[] args) {
        Config.FilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/AnkiClone/config.json";
        CurrentConfig = Config.LoadConfig();

        CardManager.Instance!.LoadCards();

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
