using DataServiceBus.OfflineHelper.DataTypes.Entity;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.Controls;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.ViewModels;
using StemmonsMobile.ViewModels.EntityViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CampusPage : ContentPage
    {
        private EntityListViewModel _entitylistVM = new EntityListViewModel();

        public EntityListViewModel EntityListVM
        {
            get
            {
                if (_entitylistVM == null)
                    _entitylistVM = new EntityListViewModel();
                return _entitylistVM;
            }

            set
            {
                if (_entitylistVM == value)
                    return;
                _entitylistVM = value;
            }
        }


        public CampusPage(EntityClass _entdetail)
        {
            InitializeComponent();
            try
            {
                Task.Run(() =>
                {
                    var res = EntitySyncAPIMethods.GetEntityByEntityID(App.Isonline, _entdetail.EntityID.ToString(), Functions.UserName, _entdetail.EntityTypeID.ToString(), App.DBPath);
                    res.Wait();
                    _entdetail = res.Result;
                }).Wait();

                EntityFieldListView ent = new EntityFieldListView(_entdetail, new List<string>() { "EXTPK", "TITLE" }, new List<string>() { "1769" });

                EntityListVM.EntityID = _entdetail.EntityID;
                EntityListVM.ScreenCode = "CAMPSRELATED";
                EntityListVM.GetEntityListwithCall().Wait();

                Label lb1 = new Label();
                lb1.Text = _entdetail.EntityTitle;
                lb1.FontSize = 17;
                lb1.FontAttributes = FontAttributes.Bold;
                lb1.HorizontalOptions = LayoutOptions.Center;
                lb1.HorizontalTextAlignment = TextAlignment.Center;
                lb1.Margin = new Thickness(0, 15, 0, 15);


                EntityListCustomControl lstEntity = new EntityListCustomControl();//Listview for Related Entity
                lstEntity.BindingContext = EntityListVM;

                lstEntity.Header = new StackLayout()
                {
                    Children =
                    {
                        lb1,
                        ent,
                        new BoxView()
                        {
                            BackgroundColor =Color.Gray,
                            HeightRequest =2
                        },
                        new Label()
                        {
                            Text ="Property",
                            FontSize =18,
                            Margin=new Thickness(5),
                            FontAttributes = FontAttributes.Bold
                        },
                        new BoxView()
                        {
                            BackgroundColor = Color.Gray,
                            HeightRequest = 2
                        }
                    }
                };

                frmList.Children.Add(lstEntity);

                BindingContext = _entdetail;

                //var ls = ent.FindByName("cntList") as ListView;
                //var tl = ls.ItemsSource as ObservableCollection<AssociationField>;
                //frmField.HeightRequest = (42 * tl.Count) + (10 * tl.Count);

            }
            catch (Exception e)
            {
            }
        }
    }

}