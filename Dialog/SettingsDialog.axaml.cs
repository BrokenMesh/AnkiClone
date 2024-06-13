using AnkiClone.Logic;
using AnkiClone.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using System.Threading.Tasks;

namespace AnkiClone;

public partial class SettingsDialog : Window
{
    public SettingsDialog() {
        InitializeComponent();

        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        this.SaveButton.Click += (_,_) => Save();
        this.OpenCardPathButton.Click += (_,_) => OpenCardPath();
        this.CardPathInput.TextChanged += (_, _) => React();
        this.CardDelayInpit.ValueChanged += (_, _) => React();

        if (!Design.IsDesignMode) {
            this.CardPathInput.Text = Program.CurrentConfig.CardStore;
            this.CardDelayInpit.Value = Program.CurrentConfig.MaxCardDelayForDue_Min;
        }
    }

    public void Save() {
        Config _c = Program.CurrentConfig;
        _c.CardStore = this.CardPathInput.Text!;
        _c.MaxCardDelayForDue_Min = (int)this.CardDelayInpit.Value!;

        Config.SaveConfig(_c);
        Config.LoadConfig(); // Reload to be sure.

        this.Close();
    }

    public async void OpenCardPath() {
        Window _window = Program.GetMainWindow()!;
        var _file = await _window.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions {
            Title = "CardStore",
            SuggestedFileName = "cardstore",
            DefaultExtension = "json"
        });
        if (_file == null)
            return;

        string? _localPath = _file.TryGetLocalPath();
        if (_localPath == null)
            return;

        this.CardPathInput.Text = _localPath;
    }

    public void React() {
        this.SaveButton.IsEnabled =
            !string.IsNullOrEmpty(this.CardPathInput.Text) &&
            CardDelayInpit.Value != null;
    }

    public static async void Open(Window _owner) {
        var dialog = new SettingsDialog();
        await dialog.ShowDialog(_owner);
    }
}