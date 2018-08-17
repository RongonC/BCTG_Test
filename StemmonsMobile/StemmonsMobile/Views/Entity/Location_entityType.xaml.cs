using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.Entity
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Location_entityType : ContentPage
    {
        public Location_entityType()
        {
            InitializeComponent();



            List<EntityTypeDetails> lst = new List<EntityTypeDetails>
            {
                new EntityTypeDetails("Active","884", "Assets/dropdowniconClose.png"),
                new EntityTypeDetails("Inactive","8", "Assets/dropdowniconClose.png"),
                new EntityTypeDetails("Total", "12","Assets/dropdowniconClose.png"),
                new EntityTypeDetails("Assigned To Me", "12","Assets/dropdowniconClose.png"),
                new EntityTypeDetails("Created By Me","32", "Assets/dropdowniconClose.png"),
                new EntityTypeDetails("owned By Me","42", "Assets/dropdowniconClose.png"),
                new EntityTypeDetails("Associated By Me","12", "Assets/dropdowniconClose.png"),
                new EntityTypeDetails("Inactivated By Me","125", "Assets/dropdowniconClose.png"),
                new EntityTypeDetails("Owner", "Vishal","Assets/dropdowniconClose.png"),
                new EntityTypeDetails("Newest", "1662","Assets/dropdowniconClose.png"),
                new EntityTypeDetails("Created New location", "","Assets/list_icon.png")
            };

            list_LocationEntity.ItemsSource = lst;
        }


        private async void List_LocationEntity_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ListView l = (ListView)sender;
            var sd = (EntityTypeDetails)l.SelectedItem;

            if (sd.Name == "Created New location")
            {
                await Navigation.PushAsync(new Add_newChillar());
            }

        }
    }
}
