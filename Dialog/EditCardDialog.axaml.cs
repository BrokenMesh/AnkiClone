using AnkiClone.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;

namespace AnkiClone;

public partial class EditCardDialog : Window {
    public bool Success;
    public Card Result;

    public EditCardDialog(string _front, string _back, string _buttonText) {
        InitializeComponent();

        this.FrontTextBox.Text = _front;
        this.BackTextBox.Text = _back;
        this.SubmitButton.Content = _buttonText;

        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        this.SubmitButton.Click += (_,_) => Submit();

        this.FrontTextBox.TextChanged += (_, _) => ButtonReact();
        this.BackTextBox.TextChanged  += (_, _) => ButtonReact();

        ButtonReact();

        Success = false;
        Result = new Card();
    }

    private void Submit() {
        Result = new Card(FrontTextBox.Text!, BackTextBox.Text!);
        Success = true;
        this.Close();
    }

    private void ButtonReact() {
        this.SubmitButton.IsEnabled = !string.IsNullOrEmpty(FrontTextBox.Text) && !string.IsNullOrEmpty(BackTextBox.Text);
    }

    public static async Task<Card?> TryCreate(Window _owner) {
        var dialog = new EditCardDialog("", "", "Create");
        await dialog.ShowDialog(_owner);

        if (dialog.Success) {
            return dialog.Result;
        }

        return null;
    }

    public static async Task<Card?> TryEdit(Window _owner, Card _old) {
        var dialog = new EditCardDialog(_old.Front, _old.Back, "Edit");
        await dialog.ShowDialog(_owner);

        if (dialog.Success) {
            return dialog.Result;
        }

        return null;
    }
}