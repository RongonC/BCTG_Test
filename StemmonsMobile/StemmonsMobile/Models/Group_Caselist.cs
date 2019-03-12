using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StemmonsMobile.DataTypes.DataType.Cases.GetCaseTypesResponse;

namespace StemmonsMobile.Models
{
    public class Group_Caselist : ObservableCollection<BasicCase>, INotifyPropertyChanged
    {

        private bool _expanded;

        public string Title { get; set; }

        public string TitleWithItemCount
        {
            get { return string.Format("{0}", Title); }
        }

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

        public int FoodCount { get; set; }
        public Group_Caselist()
        {
        }
        public Group_Caselist(string title, bool expanded = true)
        {
            Title = title;
            Expanded = expanded;
        }

        public static ObservableCollection<BasicCase> All { private set; get; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
