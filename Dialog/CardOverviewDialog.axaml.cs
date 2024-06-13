using AnkiClone.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AnkiClone;

public partial class CardOverviewDialog : Window
{
    public CardOverviewDialog()
    {
        InitializeComponent();

        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        this.Data.ItemsSource = CardManager.Instance.Cards;
    }

    public static async void Open(Window _owner) {
        var dialog = new CardOverviewDialog();
        await dialog.ShowDialog(_owner);
    }
}