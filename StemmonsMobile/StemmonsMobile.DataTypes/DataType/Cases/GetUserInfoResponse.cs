
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetUserInfoResponse : Response
    {
        public object ResponseContent { get; set; }
        #region UserInfo
        // [Serializable] 
        public class UserInfo : INotifyPropertyChanged
        {
            private string _email;

            public int ID { get; set; }
            public string MiddleName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string PrimaryJobTitle { get; set; }
            public string CellPhone { get; set; }
            public string Department { get; set; }
            public string OfficePhone { get; set; }
            public string Email
            {
                get
                {
                    OnPropertyChanged("Email");
                    return _email;
                }

                set => _email = value;
            }
            public string City { get; set; }
            public string State { get; set; }
            public string CityState
            {
                get
                {
                    return City + ", " + State;
                }
            }
            public string DisplayName { get; set; }
            public string UserID { get; set; }
            public string Supervisor { get; set; }
            public string SupervisorSAM { get; set; }
            public string SAMName { get; set; }
            public int ASSIGNED_COUNT { get; set; }
            public int CREATED_COUNT { get; set; }
            public int OWNED_COUNT { get; set; }
            public int Team_count { get; set; }
            public int Entity_Count { get; set; }
            public string IsExternalUser { get; set; }
            public string ButtonName { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}