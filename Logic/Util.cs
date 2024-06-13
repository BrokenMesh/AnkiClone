using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiClone.Logic
{
    public class Util
    {
        public static string GetRelativeDateString(DateTime _dateTime, string _suffix) {
            TimeSpan _timeSpan = _dateTime - DateTime.Now;
            _timeSpan = _timeSpan.Duration();

            if (_timeSpan.TotalDays >= 365) {
                int years = (int)(_timeSpan.TotalDays / 365);
                return years == 1 ? $"1 year {_suffix}" : $"{years} years {_suffix}";
            }
            else if (_timeSpan.TotalDays >= 30) {
                int months = (int)(_timeSpan.TotalDays / 30);
                return months == 1 ? $"1 month {_suffix}" : $"{months} months {_suffix}";
            }
            else if (_timeSpan.TotalDays >= 7) {
                int weeks = (int)(_timeSpan.TotalDays / 7);
                return weeks == 1 ? $"1 week {_suffix}" : $"{weeks} weeks {_suffix}";
            }
            else if (_timeSpan.TotalDays >= 1) {
                int days = (int)_timeSpan.TotalDays;
                return days == 1 ? $"1 day {_suffix}" : $"{days} days {_suffix}";
            }
            else if (_timeSpan.TotalHours >= 1) {
                int hours = (int)_timeSpan.TotalHours;
                return hours == 1 ? $"1 hour {_suffix}" : $"{hours} hours {_suffix}";
            }
            else if (_timeSpan.TotalMinutes >= 1) {
                int minutes = (int)_timeSpan.TotalMinutes;
                return minutes == 1 ? $"1 minute {_suffix}" : $"{minutes} minutes {_suffix}";
            }
            else {
                int seconds = (int)_timeSpan.TotalSeconds;
                return seconds <= 1 ? "just now" : $"{seconds} seconds {_suffix}";
            }
        }
    }
}
