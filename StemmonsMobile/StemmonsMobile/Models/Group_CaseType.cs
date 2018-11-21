using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StemmonsMobile.Models
{
    public class Group_CaseType : ObservableCollection<Sel_CaseTypeList>, INotifyPropertyChanged
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
        public Group_CaseType()
        {

        }
        public Group_CaseType(string title, bool expanded = true)
        {
            Title = title;
            Expanded = expanded;
        }

        public static ObservableCollection<Sel_CaseTypeList> All { private set; get; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Sel_CaseTypeList
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public bool TypeFlag { get; set; }
    }
}
