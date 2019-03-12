using DataServiceBus.OfflineHelper.DataTypes.Entity;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.CustomControls;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.Models;
using StemmonsMobile.Views.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StemmonsMobile.ViewModels
{
    public class CampusPageViewModel : INotifyPropertyChanged
    {
        // public EntityDetails edetails;
        public EntityName eClassName;

        private List<EntityName> _entityName;
        public List<EntityName> entityName
        {
            get { return _entityName; }
            set
            {
                if (value != null)
                {
                    _entityName = value;
                    // OnPropertyChanged(nameof(EntityAssocOrder));
                }
            }
        }

        public int Cheights { get; set; }

        private string _entityTtl;
        public string entityTtl
        {
            get { return _entityTtl; }
            set { _entityTtl = value; }
        }
        private int _heights;
        public int Heights
        {
            get { return _heights; }
            set
            {
                if (_heights != value)
                {
                    _heights = value;
                    OnPropertyChanged(nameof(Heights));
                }
            }
        }

        private string _ename;

        public EntityClass EntityDetails { get; set; }

        public string ename
        {
            get { return eClassName.Ename; }
            set { eClassName.Ename = value; }
        }

        public CampusPageViewModel(EntityClass _entdetail)
        {
            _entityTtl = _entdetail.EntityTitle;  // get entity title from VM
                                                  // Cheights = EntityFieldViewModel.Heights;

            var res = EntitySyncAPIMethods.GetEntityByEntityID(true, _entdetail.EntityID.ToString(), Functions.UserName, _entdetail.EntityTypeID.ToString(), App.DBPath);
            res.Wait();
            EntityDetails = res.Result;



            _entityName = new List<EntityName>()
            {
                new EntityName
                {
                    Ename ="AssocEntity"
                },
                new EntityName
                {
                    Ename ="Kalpesh_entity"
                },
                new EntityName
                {
                    Ename ="Bipin_test_Entity"
                },
                new EntityName
                {
                    Ename ="Lead_Vishalpr"
                }
                //new EntityName
                //{
                //    Ename ="AssocEntity"
                //},
                //new EntityName
                //{
                //    Ename ="Kalpesh_entity"
                //},
                //new EntityName
                //{
                //    Ename ="Bipin_test_Entity"
                //},
                //new EntityName
                //{
                //    Ename ="Lead_Vishalpr"
                //}
            };

            if (_entityName.Count > 0)
            {
                _heights = (_entityName.Count * 100) / 2;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class EntityName
    {
        public string Ename { get; set; }
    }
}
