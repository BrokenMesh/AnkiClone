using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiClone.ViewModels
{
    public partial class CardViewModel : ObservableObject
    {
        [ObservableProperty] private string cardFront = "front";
        [ObservableProperty] private string cardBack = "back";

        private int currentCardId;

        public CardViewModel() {
            CardManager.Instance.Cards.CollectionChanged += (_, _) => HandleCardChange();
            HandleCardChange();
        }

        private void HandleCardChange() {
            if (CardManager.Instance.NumberOfDueCards() == 0) return;

            currentCardId = (int)CardManager.Instance.GetEarliestCard()!;
            CardFront = CardManager.Instance.Cards[currentCardId].Front;
            CardBack = CardManager.Instance.Cards[currentCardId].Back;
        }

        public void Grade(int _grade) {
            CardManager.Instance.GradeCard(currentCardId, _grade);
        }

    }
}
