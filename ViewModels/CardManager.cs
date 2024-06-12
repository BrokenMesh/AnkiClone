using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiClone.ViewModels
{
    public class CardManager
    {
        public ObservableCollection<Card> Cards;

        private static CardManager? _instance = null;
        public static CardManager Instance
        {
            get
            {
                return _instance ?? (_instance = new CardManager());
            }
        }

        public CardManager()
        {
            if (_instance != null)
                throw new Exception("Cannot create two instances of the CardManager class!");

            _instance = this;
            Cards = new ObservableCollection<Card>();
        }

        public void LoadCards() {
            // TODO: Load Cards
        }

        public void AddCard() {
            Cards.Add(new Card("What is a tree Called", "tree"));
            Console.WriteLine(Cards.Count);
        }
    }
}
