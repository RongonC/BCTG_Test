using StemmonsMobile.DataTypes.DataType.Cases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StemmonsMobile.DataTypes.DataType.Cases.GetCaseNotesResponse;

namespace StemmonsMobile.Models
{

    public class CasesNotesGroup : ObservableCollection<NoteData>, INotifyPropertyChanged
    {
        private bool _expanded;

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
        public string Title { get; set; }
        public string CreatedDate { get; set; }
        public string Uname { get; set; }
        public string StateIcon
        {
            get { return Expanded ? "Assets/dropdownicon.png" : "Assets/dropdowniconClose.png"; }
        }

        // public int FoodCount { get; set; }
        public CasesNotesGroup(string title, string createdDate, string uname)
        {
            Title = title;
            CreatedDate = createdDate;
            Uname = "By " + uname;
        }

        public static ObservableCollection<ChillerEntityGroup> All { private set; get; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
