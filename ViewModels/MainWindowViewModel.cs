using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AnkiClone.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty] private bool hasCard = false;

        public MainWindowViewModel() {
            CardManager.Instance.Cards.CollectionChanged += (_,_) => HandleCardChange();
            HandleCardChange();
        }

        public void AddCard() {
            CardManager.Instance.AddCard();   
        }

        private void HandleCardChange() {
            HasCard = CardManager.Instance.Cards.Count != 0;
        }
    }
}
