using DataServiceBus.OfflineHelper.DataTypes.Cases;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StemmonsMobile.Commonfiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static StemmonsMobile.DataTypes.DataType.Cases.OriginationCenterDataResponse;

namespace StemmonsMobile.ViewModels
{
    public class CustomlistViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<OriginationCenterData> _list;

        public ObservableCollection<OriginationCenterData> MainList
        {
            get { return _list; }
            set { _list = value; }
        }

        public CustomlistViewModel()
        {
            Task<List<OriginationCenterData>> results = CasesSyncAPIMethods.GetOriginationCenterForUser(Functions.UserName, App.DBPath);
            results.Wait();
            MainList = new ObservableCollection<OriginationCenterData>(results.Result);
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public ICommand ItemTapped
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    await RefreshData();

                    IsRefreshing = false;
                });
            }
        }

        int c = 0;
        private Task RefreshData()
        {
            MainList.Insert(0, new OriginationCenterData() { CaseTypeID = 0, CaseTypeName = "Testt " + c++ });
            return null;
        }

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                NotifyPropertyChanged(nameof(IsRefreshing));
            }
        }
    }
}
