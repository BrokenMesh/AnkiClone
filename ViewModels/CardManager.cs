using AnkiClone.Logic;
using Avalonia.Controls;
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
        public BatchObservableCollection<Card> Cards;

        private static CardManager? _instance = null;
        public static CardManager Instance {
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
            Cards = new BatchObservableCollection<Card>();

            if (Design.IsDesignMode) {
                Cards.Add(new Card("1", "tree"));
                Cards.Add(new Card("2", "tree"));
                Cards.Add(new Card("3", "tree"));
                Cards.Add(new Card("4", "tree"));
            }
        }

        public void LoadCards() {
            string _file = File.ReadAllText(Program.CurrentConfig.CardStore + "/cardstore.json");
            Cards = JsonSerializer.Deserialize<BatchObservableCollection<Card>>(_file)!;
        }

        public void SaveCards() {
            string _file = JsonSerializer.Serialize(Cards);
            File.WriteAllText(Program.CurrentConfig.CardStore + "/cardstore.json", _file);
        }

        public void DumpCards(string _path) {
            string _file = JsonSerializer.Serialize(Cards);
            File.WriteAllText(_path, _file);
        }

        public void LoadCardDump(string _path) {
            string _file = File.ReadAllText(_path);
            Cards.Clear();
            Cards.AddRange(JsonSerializer.Deserialize<List<Card>>(_file)!);
        }

        public void AddCard(Card _card) {
            Cards.Add(_card);
        }

        public void RemoveCard(int _id) {
            Cards.RemoveAt(_id);
        }

        public void EditCard(Card _card, int _id) {
            Card _c = Cards[_id];

            _c.Front = _card.Front;
            _c.Back = _card.Back;

            Cards[_id] = _c;
        }

        public int NumberOfDueCards() {
            int _n = 0;

            foreach (Card _c in Cards) {
                if (_c.IsDue() && _c.IsEnabled) {
                    _n++;
                }
            }

            return _n;
        }

        public int GetEarliestCard() {
            DateTime _lowest = DateTime.MaxValue;
            int _id = -1;

            for (int i = 0; i < Cards.Count; i++) {
                if (Cards[i].DueAt() < _lowest && Cards[i].IsEnabled) {
                    _id = i;
                    _lowest = Cards[i].DueAt();
                }
            }

            return _id;
        }

        public void GradeCard(int _cardId, int _grade) {
            Cards[_cardId] = Grade(Cards[_cardId], _grade);
        }

        // https://en.wikipedia.org/wiki/SuperMemo
        private Card Grade(Card _card, int _grade) {

            if (_grade >= 3) { // correct response
                if (_card.Repetitons == 0) {
                    _card.Interval_Min = 1;
                }
                else if (_card.Repetitons == 1) {
                    _card.Interval_Min = 6;
                }
                else {
                    _card.Interval_Min = (int)(_card.Interval_Min * _card.Easyness);
                }

                _card.Repetitons++;
            }
            else { // incorrect response
                _card.Repetitons = 0;
                _card.Interval_Min = 1;
            }

            float _inverseGrade = 5 - _grade;
            _card.Easyness = _card.Easyness + (0.1f - _inverseGrade * (0.08f + _inverseGrade * 0.02f));

            if (_card.Easyness < 1.3f) _card.Easyness = 1.3f;

            _card.LastChecked = DateTime.Now;

            return _card;
        }

    }
}
