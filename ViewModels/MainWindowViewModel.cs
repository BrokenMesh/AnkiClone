using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Controls;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System.IO;

namespace AnkiClone.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty] private bool hasCard = false;
        [ObservableProperty] private int numberDueCards = 1;

        public MainWindowViewModel() {

            if (!Design.IsDesignMode) {
                try {
                    CardManager.Instance!.LoadCards();
                } catch (FileNotFoundException _e) {
                    MessageBoxManager.GetMessageBoxStandard(
                        "Warning",
                        "No Cardstore found. Creating new Cardstore!",
                        ButtonEnum.Ok, Icon.Warning
                    ).ShowAsync();
                } catch (Exception _e) {
                    MessageBoxManager.GetMessageBoxStandard(
                        "Error",
                        "Unable to load Cards:\nError: " + _e.Message,
                        ButtonEnum.Ok, Icon.Error
                    ).ShowAsync();
                }
            }

            CardManager.Instance.Cards.CollectionChanged += (_,_) => HandleCardChange();
            HandleCardChange();
        }

        public void AddCard() {
            CardManager.Instance.AddCard();
            SaveCards();
        }

        public void SaveCards() {
            if(!Design.IsDesignMode) {
                try {
                    CardManager.Instance.SaveCards();
                } catch (Exception _e) {
                    MessageBoxManager.GetMessageBoxStandard(
                        "Error",
                        "Unable to save Cards:\nError: " + _e.Message, 
                        ButtonEnum.Ok, Icon.Error
                    ).ShowAsync();
                }
            }
        }

        private void HandleCardChange() {
            NumberDueCards = CardManager.Instance.NumberOfDueCards();
            HasCard = NumberDueCards != 0;
        }
    }
}
