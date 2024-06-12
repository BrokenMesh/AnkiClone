using AnkiClone.Logic;
using AnkiClone.ViewModels;
using Avalonia.Controls;
using System.ComponentModel.DataAnnotations;

namespace AnkiClone;

public partial class MainWindow : Window
{
    public MainWindow() {
        InitializeComponent();
        var mv = this.DataContext as MainWindowViewModel;
        this.Closing += (_,_) => mv.SaveCards();
    }
}