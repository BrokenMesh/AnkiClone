using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System.IO;
using static System.Net.WebRequestMethods;
using System.Security.Cryptography;

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

        public void Settings() {
            Window _window = Program.GetMainWindow()!;
            SettingsDialog.Open(_window);
        }

        public void CardOverview() {
            Window _window = Program.GetMainWindow()!;
            CardOverviewDialog.Open(_window);
        }

        public async void AddCard() {
            Window _window = Program.GetMainWindow()!;
            Card? _card = await EditCardDialog.TryCreate(_window);

            if (_card == null) return;

            CardManager.Instance.AddCard((Card)_card);

            SaveCards();
        }

        public async void EditCard() {
            int _id = CardManager.Instance.GetEarliestCard();

            Window _window = Program.GetMainWindow()!;
            Card? _card = await EditCardDialog.TryEdit(_window, CardManager.Instance.Cards[_id]);

            if (_card == null) return;

            CardManager.Instance.EditCard((Card)_card, _id);

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

        public void ReloadCards() {
            CardManager.Instance.Cards.Refresh();
        }

        public void ResetCurrentCard() {
            if (CardManager.Instance.NumberOfDueCards() == 0) return;

            int _id = CardManager.Instance.GetEarliestCard();

            Card _c = CardManager.Instance.Cards[_id];
            CardManager.Instance.Cards[_id] = new Card(_c.Front, _c.Back);
        }

        public void DisableCurrentCard() {
            if (CardManager.Instance.NumberOfDueCards() == 0) return;

            int _id = CardManager.Instance.GetEarliestCard();

            Card _c = CardManager.Instance.Cards[_id];
            _c.IsEnabled = false;
            CardManager.Instance.Cards[_id] = _c;
        }

        public void DelayCurrentCard() {
            if (CardManager.Instance.NumberOfDueCards() == 0) return;

            int _id = CardManager.Instance.GetEarliestCard();

            Card _c = CardManager.Instance.Cards[_id];
            _c.LastChecked = DateTime.Now + TimeSpan.FromDays(1);
            CardManager.Instance.Cards[_id] = _c;
        }

        public async void DumpCardStore() {
            Window _window = Program.GetMainWindow()!;
            var _file = await _window.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions { 
                Title = "Dump CardStore",
                SuggestedFileName = "cardstore",
                DefaultExtension = "json"
            });
            if (_file == null) return;

            string? _localPath = _file.TryGetLocalPath();
            if (_localPath == null) return;

            if (!Design.IsDesignMode) {
                try {
                    CardManager.Instance.DumpCards(_localPath);
                }
                catch (Exception _e) {
                    _ = MessageBoxManager.GetMessageBoxStandard(
                        "Error",
                        "Unable to dump Cards:\nError: " + _e.Message,
                        ButtonEnum.Ok, Icon.Error
                    ).ShowAsync();
                }
            }
        }

        public async void LoadCardStore() {
            Window _window = Program.GetMainWindow()!;

            var fileTypes = new List<FilePickerFileType> {
                new FilePickerFileType("JSON Files"){
                    Patterns = new[] { "*.json" }
                }
            };

            var _files = await _window.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions {
                Title = "Open CardStore Dump",
                FileTypeFilter = fileTypes,
                AllowMultiple = false,
            });

            if (_files.Count < 1) return;

            string? _localPath = _files[0].TryGetLocalPath();
            if (_localPath == null) return;

            if (!Design.IsDesignMode) {
                try {
                    CardManager.Instance.LoadCardDump(_localPath);
                }
                catch (Exception _e) {
                    _ = MessageBoxManager.GetMessageBoxStandard(
                        "Error",
                        "Unable to load Cards:\nError: " + _e.Message,
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
