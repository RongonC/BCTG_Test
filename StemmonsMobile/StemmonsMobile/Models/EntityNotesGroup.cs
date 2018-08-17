using StemmonsMobile.DataTypes.DataType.Entity;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StemmonsMobile.Views.Entity
{

    public class EntityNotesGroup : ObservableCollection<Entity_Notes>, INotifyPropertyChanged
    {
        private string _title;
        private string _createdDate;
        private string _uname;
      

        public string Title
        {
            get => _title;
            set
            {
                _title = value; OnPropertyChanged("Title");
            }
        }
        public string CreatedDate
        {
            get => _createdDate;
            set
            {
                _createdDate = value;
                OnPropertyChanged("CreatedDate");
            }
        }
        public string Uname
        {
            get => _uname; set
            {
                _uname = value;
                OnPropertyChanged("Uname");
            }
        }
        

        public EntityNotesGroup(string title, string createdDate, string uname)
        {
            Title = title;
            CreatedDate = createdDate;
            Uname = "By: " + uname;
        
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
