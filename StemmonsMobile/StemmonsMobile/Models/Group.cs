using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace StemmonsMobile.Models
{
    public class UserData : List<string>
    {
        public string Id { get; set; }
        public string Name
        {
            get;
            set;
        }
    }

    public class UserGroup : ObservableCollection<AssignCasePageData>, INotifyPropertyChanged
    {


        public string Title { get; set; }

        public UserGroup(string title)
        {
            Title = title;

        }

        public static ObservableCollection<UserGroup> All { private set; get; }

        static UserGroup()
        {
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
    
    //public class GroupData : ObservableCollection<Group>,INotifyPropertyChanged
    //{
    //    public string U_id
    //    {
    //        get;
    //        set;
    //    }
    //    public string U_name
    //    {
    //        get;
    //        set;
    //    }

    //    public GroupData(string _id,string _name)
    //    {
    //        U_id = _id;
    //        U_name = _name;

    //    }
    //}

}

