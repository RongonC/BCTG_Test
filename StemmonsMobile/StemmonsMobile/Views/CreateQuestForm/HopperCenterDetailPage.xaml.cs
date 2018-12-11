using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Quest;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Quest;
using StemmonsMobile.Views.LoginProcess;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.CreateQuestForm
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HopperCenterDetailPage : ContentPage
    {
        GetCountForQuestItemResponse.CountForQuestItem questcount;
        public HopperCenterDetailPage()
        {
            InitializeComponent();
            App.SetConnectionFlag();
			try
			{
				var callreply = QuestSyncAPIMethods.GetCountForQuestItem(App.Isonline, Convert.ToString(Functions.tempitemid), Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
				questcount = callreply.Result;

				List<Dialogdata> lst = new List<Dialogdata>{
				new Dialogdata("Pending",Convert.ToString(questcount.Pending)),
				new Dialogdata("Finalized",Convert.ToString(questcount.Finalized)),
				new Dialogdata("Total",Convert.ToString(questcount.Total)),
				new Dialogdata("My Pending",Convert.ToString(questcount.MyPending)),
				new Dialogdata("My Finalized",Convert.ToString(questcount.MyFinalized)),
				new Dialogdata("Owner", questcount.Owner),
				new Dialogdata("Newest",questcount.Newest),
			};
				DetailList.ItemsSource = lst;
			}
			catch(Exception ex){
				
			}
            
        }
        public HopperCenterDetailPage(string type) : this()
        {
            Title = $"{type} Form";
            //CreateFormName.Text = "Create New " + $"{type} Form";            
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                this.Navigation.PushAsync(new NewQuestForm(Functions.tempitemid));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void DetailList_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            try
            {
                ListView l = (ListView)sender;
                var sd = (Dialogdata)l.SelectedItem;

                if (sd.Category.ToLower().Trim() == "pending")
                {
                    this.Navigation.PushAsync(new QuestItemPage("detailpage", Functions.UserName, "pending", Convert.ToString(Functions.tempitemid)));
                }
                else if (sd.Category.ToLower().Trim() == "finalized")
                {
                    this.Navigation.PushAsync(new QuestItemPage("detailpage", Functions.UserName, "finalized", Convert.ToString(Functions.tempitemid)));
                }
                else if (sd.Category.ToLower().Trim() == "total")
                {
                    this.Navigation.PushAsync(new QuestItemPage("detailpage", Functions.UserName, "total", Convert.ToString(Functions.tempitemid)));
                }
                else if (sd.Category.Trim() == "MyPending")
                {
                    this.Navigation.PushAsync(new QuestItemPage("detailpage", Functions.UserName, "MyPending", Convert.ToString(Functions.tempitemid)));
                }
                else if (sd.Category == "My Finalized")
                {
                    this.Navigation.PushAsync(new QuestItemPage("detailpage", Functions.UserName, "MyFinalized", Convert.ToString(Functions.tempitemid)));
                }
            }
            catch (Exception ex)
            {

                
            }
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

        private  void btn_more_Clicked(object sender, EventArgs e)
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

    public class Dialogdata
    {
        public string Category { get; set; }
        public string Details { get; set; }
        public Dialogdata(String category, string detail)
        {
            Category = category;
            Details = detail;
        }
    }

    public class QuestCountData
    {

        public string Pending { get; set; }
        public string Finalized { get; set; }
        public string Total { get; set; }
        public string MyPending { get; set; }
        public string MyFinalized { get; set; }
        public string Owner { get; set; }
        public string Newest { get; set; }


    }
}
