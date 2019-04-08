using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.Controls;
using StemmonsMobile.DataTypes.DataType;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PropertyInfoPage : ContentPage
    {
        public EntityClass EntityLists = new EntityClass();

        public string fileStr = string.Empty;
        public PropertyInfoPage(EntityClass _entityClass)
        {
            InitializeComponent();
            EntityLists = _entityClass;
            this.BindingContext = EntityLists;

            try
            {
                //ProfileImg.Source = Functions.GetImageFromEntityAssoc(EntityLists.AssociationFieldCollection);
            }
            catch (Exception)
            {
                //ProfileImg.Source = "Assets/na.png";
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            EntityFieldListView entityfieldcontrol = new EntityFieldListView(EntityLists, new List<string>() { "TITLE" }, new List<string>() { "1814", "1815", "1816", "1819", "1820", "1821", "1822", "1823", "1824", "1825", "1826", "1827", "1829", "1830", "1833", "1834", "1836", "1842" });

            var ls = entityfieldcontrol.FindByName("cntList") as ListView;

            StackLayout header = new StackLayout();

            Label lb1 = new Label();
            lb1.Text = EntityLists.EntityTitle;
            lb1.FontSize = 17;
            lb1.FontAttributes = FontAttributes.Bold;
            lb1.HorizontalOptions = LayoutOptions.Center;
            lb1.HorizontalTextAlignment = TextAlignment.Center;
            lb1.Margin = new Thickness(0, 15, 0, 15);

            Image img1 = new Image();
            img1.HeightRequest = 200;
            try
            {
                img1.Source = Functions.GetImageFromEntityAssoc(EntityLists.AssociationFieldCollection);
            }
            catch (Exception)
            {
                img1.Source = "Assets/na.png";
            }

            header.Children.Add(lb1);
            header.Children.Add(img1);

            ls.Header = header;

            controlFrame.Children.Add(entityfieldcontrol);
            ls.SelectedItem = null;
        }
    }
}