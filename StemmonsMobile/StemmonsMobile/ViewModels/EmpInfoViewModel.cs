using DataServiceBus.OfflineHelper.DataTypes.Entity;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.Views.People_Screen;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace StemmonsMobile.ViewModels
{
    public class EmpInfoViewModel : INotifyPropertyChanged
    {

        private bool _isbusy;

        public bool isbusy
        {
            get { return _isbusy; }
            set
            {
                _isbusy = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("isbusy"));
            }
        }

        public EntityClass SelectedEntity { get; set; }

        public string EntityId { get; set; } = string.Empty;

        private EntityRoleRelationAssignment _selectedItemsEntity;

        public EntityRoleRelationAssignment SelectedItemsEntity
        {
            get => _selectedItemsEntity;

            set
            {
                if (_selectedItemsEntity != value)
                {
                    _selectedItemsEntity = value;
                    OnPropertyChanged("SelectedItemsEntity");
                    EntitySelectedItem(_selectedItemsEntity);
                }

            }
        }
        public EmpInfoViewModel(string id)
        {
            EntityId = id;

            Employee_Role_List = new ObservableCollection<EntityRoleRelationAssignment>();
            ObservableCollection<EntityRoleRelationAssignment> _List = new ObservableCollection<EntityRoleRelationAssignment>();
            isbusy = true;

            Task.Run(() =>
            {
                PopulateList(out _List);
                Employee_Role_List = _List;
            });



            this.LoadDataCommand = new Command(async () =>
            {

                ////var newNews = repo.getNews(1);
                //PageIndex += 1;

                //{
                //    IsShow = true;
                //    await GetEntityListwithCall();
                //    IsShow = false;
                //}

                //foreach (var item in newNews)
                //{
                //    News.Add(item);
                //    OnpropertyChanged("News");
                //}
            });
        }

        public ICommand LoadDataCommand
        {
            get;
            set;
        }

        private async void EntitySelectedItem(EntityRoleRelationAssignment obj)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new UserDetail(obj.Username));
        }

        private ObservableCollection<EntityRoleRelationAssignment> _employee_Role_List;
        public ObservableCollection<EntityRoleRelationAssignment> Employee_Role_List
        {
            get { return _employee_Role_List; }
            set
            {
                _employee_Role_List = value;
                OnPropertyChanged(nameof(Employee_Role_List));

            }
        }
        void PopulateList(out ObservableCollection<EntityRoleRelationAssignment> lst)
        {
            ObservableCollection<EntityRoleRelationAssignment> lt = new ObservableCollection<EntityRoleRelationAssignment>();
            Task.Run(() =>
              {
                  lt = EntitySyncAPIMethods.GetEntityRoleRelationData(App.Isonline, EntityId, Functions.UserName, App.DBPath);
              }).Wait();
            isbusy = false;
            lst = lt;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string v)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(v));
            }
        }
    }
}



