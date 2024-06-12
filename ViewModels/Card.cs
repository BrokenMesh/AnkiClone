using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiClone.ViewModels
{
    public class Card
    {
        public string Front { get; set; }
        public string Back { get; set; }
        public int Repetitons { get; set; }
        public float Easyness { get; set; }
        public float Interval_Days { get; set; }
        public DateTime LastChecked { get; set; }

        public Card(string front, string back) {
            Front = front;
            Back = back;

            Repetitons = 0;
            Easyness = 2.5f;
            Interval_Days = 0f;
            LastChecked = DateTime.Now - TimeSpan.FromMinutes(1);
        }
    }
}
