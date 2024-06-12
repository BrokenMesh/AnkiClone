using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiClone.ViewModels
{
    public struct Card
    {
        public string Front { get; set; }
        public string Back { get; set; }
        public int Repetitons { get; set; }
        public float Easyness { get; set; }
        public float Interval_Min { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime LastChecked { get; set; }

        public Card(string front, string back) {
            Front = front;
            Back = back;

            Repetitons = 0;
            Easyness = 2.5f;
            Interval_Min = 0f;
            IsEnabled = true;
            LastChecked = DateTime.Now - TimeSpan.FromMinutes(1);
        }

        public DateTime DueAt() => LastChecked + TimeSpan.FromMinutes(Interval_Min);
        public bool IsDue() => DueAt() < DateTime.Now + TimeSpan.FromMinutes(Program.CurrentConfig.MaxCardDelayForDue_Min); 
        
    }
}
