using AnkiClone.Logic;
using AnkiClone.ViewModels;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using System;

namespace AnkiClone;

class Program
{
    public static Config CurrentConfig { get; set; }


    [STAThread]
    public static void Main(string[] args) {
        Config.FilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/AnkiClone/config.json";
        CurrentConfig = Config.LoadConfig();

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static Window? GetMainWindow() {
        return Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : null;
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp() {

        if (Design.IsDesignMode) {
            Config.FilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/AnkiClone/config.json";
            CurrentConfig = Config.LoadConfig();
        }

        return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }
}
