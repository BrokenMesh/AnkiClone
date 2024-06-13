using AnkiClone.Logic;
using AnkiClone.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.VisualBasic;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnkiClone;

public partial class CardOverviewDialog : Window
{
    public CardOverviewDialog() {
        InitializeComponent();

        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        this.EditButton.Click += (_, _) => Edit();
        this.ToggleButton.Click += (_, _) => Toggle();
        this.ResetButton.Click += (_, _) => Reset();
        this.DeleteButton.Click += (_, _) => Delete();
        this.CloseButton.Click += (_, _) => this.Close();

        this.Data.SelectionChanged += (_, _) => DataReact();
        CardManager.Instance.Cards.CollectionChanged += (_,_) => CardReact();
        
        CardReact();
        DataReact();
    }

    public void CardReact() {
        if (Design.IsDesignMode) {
            this.Data.ItemsSource = CardManager.Instance.Cards;
            return;
        }

        List<CardItem> _visual = CardManager.Instance.Cards.Select((_c, _i) => {
            return new CardItem(_c, _i);
        }).ToList();

        this.Data.ItemsSource = _visual;
    }

    public void DataReact() {
        CardButtons.IsEnabled = this.Data.SelectedIndex != -1;
    }

    public async void Edit() {
        int _id = this.Data.SelectedIndex;

        Window _window = Program.GetMainWindow()!;
        Card? _card = await EditCardDialog.TryEdit(_window, CardManager.Instance.Cards[_id]);

        if (_card == null) return;

        CardManager.Instance.EditCard((Card)_card, _id);

        SaveCards();
    }

    public void Toggle() {
        int _id = this.Data.SelectedIndex;

        Card _c = CardManager.Instance.Cards[_id];
        _c.IsEnabled = !_c.IsEnabled;
        CardManager.Instance.Cards[_id] = _c;

        SaveCards();
    }

    public void Reset() {
        int _id = this.Data.SelectedIndex;

        Card _c = CardManager.Instance.Cards[_id];
        CardManager.Instance.Cards[_id] = new Card(_c.Front, _c.Back);

        SaveCards();
    }

    public void Delete() {
        CardManager.Instance.RemoveCard(this.Data.SelectedIndex);

        SaveCards();
    }

    public void SaveCards() {
        if (!Design.IsDesignMode) { // TODO: Repeating Code, Remove 
            try {
                CardManager.Instance.SaveCards();
            }
            catch (Exception _e) {
                MessageBoxManager.GetMessageBoxStandard(
                    "Error",
                    "Unable to save Cards:\nError: " + _e.Message,
                    ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info
                ).ShowAsync();
            }
        }
    }

    public static async void Open(Window _owner) {
        var dialog = new CardOverviewDialog();
        await dialog.ShowDialog(_owner);
    }
}

public class CardItem
{
    public int Id { get; set; }
    public string Front { get; set; }
    public string Back { get; set; }
    public string Due { get; set; }
    public int Streak { get; set; }
    public bool IsEnabled { get; set; }

    public CardItem(Card _card, int _id) {
        Id = _id;
        Front = LimitLength(_card.Front);
        Back = LimitLength(_card.Back);
        IsEnabled = _card.IsEnabled;
        Streak = _card.Repetitons;

        if (_card.IsDue()) {
            Due = "Is Due";
        }
        else {
            Due = "in " + Util.GetRelativeDateString(_card.DueAt(), "");
        }
    }

    private string LimitLength(string _s) {
        return _s.Length > 20 ? _s.Substring(0, 20) + " ..." : _s;
    }
}