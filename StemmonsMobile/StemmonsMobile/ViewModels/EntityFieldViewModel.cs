using DataServiceBus.OfflineHelper.DataTypes.Entity;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.Models;
using StemmonsMobile.Views.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StemmonsMobile.ViewModels
{
    public class EntityFieldViewModel : INotifyPropertyChanged
    {
        public bool Onlineflag = true;
        public EntityClass EntityLists = new EntityClass();
        public AssociationMetaData metaData;

        public List<string> AcSystemCode = new List<string>();// { "EXTPK", "EPILR" }; // hardcoded lists
        public List<string> AcTypeId = new List<string>();// { "24", "25", "26", "27", "28", "29", "30", "32", "31" }; // hardcoded lists

        #region Properties
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

        private string _Fname;
        public string Fname
        {
            get { return metaData.FieldName; }
            set { metaData.FieldName = value; }
        }

        private string _Fvalue;
        public string Fvalue
        {
            get { return metaData.FieldValue; }
            set { metaData.FieldValue = value; }
        }

        private static string _entitytitle;
        public static string entitytitle
        {
            get { return _entitytitle; }
            set { _entitytitle = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ObservableCollection<AssociationField> _entityAssocOrder;
        public ObservableCollection<AssociationField> EntityAssocOrder
        {
            get
            {
                return _entityAssocOrder;

            }
            set
            {
                if (value != null)
                {
                    _entityAssocOrder = value;
                    _heights = (_entityAssocOrder.Count * 100) / 2;
                }
            }
        }

        //INavigation _navigation;
        //public EntityFieldViewModel(INavigation navigation)
        //{
        //    _navigation = navigation;
        //    _entitytitle = _entityListMBView.Title;
        //} 
        #endregion

        public EntityClass SelectedEntity { get; set; }

        private bool _isbuzy;

        public bool IsBuZy
        {
            get { return _isbuzy; }
            set
            {
                _isbuzy = value;
                OnPropertyChanged(nameof(IsBuZy));
            }
        }


        public EntityFieldViewModel(EntityClass _entdetail, List<string> _acsystemcode = null, List<string> _assoctypeid = null)
        {
            SelectedEntity = _entdetail;
            AcSystemCode = _acsystemcode;
            AcTypeId = _assoctypeid;

            _entitytitle = SelectedEntity.EntityTitle;
            _entityAssocOrder = new ObservableCollection<AssociationField>();
            getEntityFieldValue();

         
        }

        public async void getEntityFieldValue() // get feild/value according AssocCode and AssocTypeId
        {
            IsBuZy = true;
            try
            {
                await Task.Run(() =>
                 {
                     var lst = EntitySyncAPIMethods.GetEntityByEntityID(Onlineflag, SelectedEntity.EntityID.ToString(), Functions.UserName, SelectedEntity.EntityTypeID.ToString(), App.DBPath);
                     lst.Wait();
                     EntityLists = lst.Result;
                 });
                if (AcSystemCode?.Count > 0 || AcTypeId?.Count > 0)
                {
                    foreach (var code in AcSystemCode)
                    {
                        AssociationField t = new AssociationField();
                        var data1 = EntityLists.AssociationFieldCollection.Where(x => x.AssocSystemCode == code).ToList();

                        if (data1.Count > 0)
                        {
                            _entityAssocOrder.Add(data1[0]);
                        }
                    }

                    foreach (var id in AcTypeId)
                    {
                        AssociationField t = new AssociationField();
                        var data1 = EntityLists.AssociationFieldCollection.Where(x => x.AssocTypeID == Convert.ToInt32(id)).ToList();

                        if (data1.Count > 0)
                        {
                            _entityAssocOrder.Add(data1[0]);
                        }
                    }
                }
                else
                {
                    _entityAssocOrder = new ObservableCollection<AssociationField>(EntityLists.AssociationFieldCollection);
                }
                if (_entityAssocOrder.Count > 0)
                {
                    _entityAssocOrder = new ObservableCollection<AssociationField>(_entityAssocOrder.OrderBy(x => x.ItemDesktopPriorityValue).ToList());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "SystemCode and TypeId does not match!", "Okay");
                }
            }
            catch (Exception)
            {
            }
            if (_entityAssocOrder.Count > 0)
            {
                Heights = (42 * _entityAssocOrder.Count) + (10 * _entityAssocOrder.Count);
            }
            IsBuZy = false;
        }
    }
}