using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiClone.Logic
{

    // ObservableCollection does not support batch adding element without causing many UI updates.
    // This class exists so solve this by halting the reactivity while adding elements.
    public class BatchObservableCollection<T> : ObservableCollection<T>
    {
        private bool _suspendNotifications;

        public void AddRange(IEnumerable<T> items) {
            _suspendNotifications = true;
            foreach (var item in items) {
                Add(item);
            }
            _suspendNotifications = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e) {
            if (!_suspendNotifications) {
                base.OnCollectionChanged(e);
            }
        }
    }
}
