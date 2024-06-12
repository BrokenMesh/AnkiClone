using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
            string _file = File.ReadAllText(Program.CurrentConfig.CardStore + "/cardstore.json");
            Cards = JsonSerializer.Deserialize<ObservableCollection<Card>>(_file)!;
        }

        public void SaveCards() {
            string _file = JsonSerializer.Serialize(Cards);
            File.WriteAllText(Program.CurrentConfig.CardStore + "/cardstore.json", _file);
        }

        public void AddCard() {
            Cards.Add(new Card("What is a tree Called", "tree"));
            //Console.WriteLine(Cards.Count);
        }

        public int NumberOfDueCards() {
            int _n = 0;

            foreach (Card _c in Cards) {
                if (_c.LastChecked + TimeSpan.FromDays(_c.Interval_Days) < DateTime.Now) {
                    _n++;
                }
            }

            return _n;
        }

        public Card? GetDueCard() {
            foreach (Card _c in Cards) {
                if (_c.LastChecked + TimeSpan.FromDays(_c.Interval_Days) < DateTime.Now) {
                    return _c;
                }
            }

            return null;
        }

    }
}
