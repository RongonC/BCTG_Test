using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;
namespace StemmonsMobile.Models
{
    public class ChillerEntityGroup : ObservableCollection<ChillerEntity>, INotifyPropertyChanged
    {
        private bool _expanded;

        public string Title { get; set; }

        public string TitleWithItemCount
        {
            get { return string.Format("{0} {1}", Title, ""); }
        }

        public string ShortName { get; set; }

        public bool Expanded
        {
            get { return _expanded; }
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                    OnPropertyChanged("Expanded");
                    OnPropertyChanged("StateIcon");
                }
            }
        }

        public string StateIcon
        {
            get { return Expanded ? "Assets/dropdownicon.png" : "Assets/dropdowniconClose.png"; }
        }

        // public int FoodCount { get; set; }

        public ChillerEntityGroup(string title, string shortName, bool expanded)
        {
            Title = title;
            ShortName = shortName;
            Expanded = expanded;
        }

        public static ObservableCollection<ChillerEntityGroup> All { private set; get; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
