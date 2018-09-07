using DataServiceBus.OnlineHelper.DataTypes;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Entity;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using StemmonsMobile.Views.LoginProcess;
using System.Threading.Tasks;
using DataServiceBus.OfflineHelper.DataTypes.Common;

namespace StemmonsMobile.Views.Entity
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Entity_TypeDetails : ContentPage
    {
        EntityOrgCenterList Selected_Entity;
        public Entity_TypeDetails(EntityOrgCenterList _Selected)
        {
            InitializeComponent();
            Title = _Selected.EntityTypeName;
            App.SetConnectionFlag();
            Selected_Entity = _Selected;
            Functions.IsEditEntity = false;
            #region GetEntityTypeDetails

            List<EntityTypeDetails> lst = new List<EntityTypeDetails>();


            lst.Add(new EntityTypeDetails("Active", Selected_Entity.ActiveCount?.ToString() ?? "", "Assets/dropdowniconClose.png"));
            lst.Add(new EntityTypeDetails("Inactive", Selected_Entity.InactiveCount?.ToString() ?? "", "Assets/dropdowniconClose.png"));
            lst.Add(new EntityTypeDetails("Total", Selected_Entity.TotalCount?.ToString() ?? "", "Assets/dropdowniconClose.png"));
            lst.Add(new EntityTypeDetails("Assigned To Me", Selected_Entity.AssignedToUser?.ToString() ?? "", "Assets/dropdowniconClose.png"));
            lst.Add(new EntityTypeDetails("Created By Me", Selected_Entity.CreatedByUser?.ToString() ?? "", "Assets/dropdowniconClose.png"));
            lst.Add(new EntityTypeDetails("Owned By Me", Selected_Entity.OwnedByUser?.ToString() ?? "", "Assets/dropdowniconClose.png"));
            lst.Add(new EntityTypeDetails("Associated By Me", Selected_Entity.ASSOCIATED_TO_USER?.ToString() ?? "", "Assets/dropdowniconClose.png"));
            lst.Add(new EntityTypeDetails("Inactivated By Me", Selected_Entity.InactivatedByUser?.ToString() ?? "", "Assets/dropdowniconClose.png"));
            lst.Add(new EntityTypeDetails("Owner", Selected_Entity.Owner?.ToString() ?? "", "Assets/dropdowniconClose.png"));
            DateTime d = Convert.ToDateTime(Selected_Entity.LastCreatedDateTime?.ToString());
            lst.Add(new EntityTypeDetails("Newest", d.ToString("d") ?? "", "Assets/dropdowniconClose.png"));
            lst.Add(new EntityTypeDetails(Selected_Entity.NewEntityText ?? "Create Entity", "", "Assets/list_icon.png"));


            List_EntityDetails.ItemsSource = lst;


            #endregion
        }

        private async void List_EntityDetails_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var btn = e.Item as EntityTypeDetails;
            if (btn.Name.ToLower().Contains("create new"))
            {
                Functions.IsEditEntity = false;
                await Navigation.PushAsync(new CreateEntityPage(Selected_Entity.EntityTypeID, Selected_Entity.EntityID, Selected_Entity.NewEntityText, null));
            }
            else if (btn.Name != "Newest" && btn.Name != "Owner" && btn.Type_value != "0")
            {
                await Navigation.PushAsync(new EntityDetailsSubtype(Selected_Entity, btn.Name));
            }

            List_EntityDetails.SelectedItem = null;
        }

        private async void Btn_CreateEntity_Clicked(object sender, EventArgs e)
        {
            Functions.IsEditEntity = false;
            await Navigation.PushAsync(new CreateEntityPage(Selected_Entity.EntityTypeID, Selected_Entity.EntityID, Selected_Entity.NewEntityText, null));
        }

        private void btn_home_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.GotoHome(this);
            }
            catch (Exception)
            {
            }
        }

        private void btn_more_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.BtnHumburger(this);
            }
            catch (Exception)
            {
            }
        }
    }
}