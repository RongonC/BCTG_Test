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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.CreateQuestForm
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HopperCenterDetailsView : ListView
    {
        QuestCountData questcount;
        public HopperCenterDetailsView()
        {
            InitializeComponent();

            App.SetConnectionFlag();
			try{
            var callreply = QuestSyncAPIMethods.GetCountForQuestItem(App.Isonline, Convert.ToString(Functions.tempitemid), Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
            
            List<Dialogdata0> lst = new List<Dialogdata0>{
                new Dialogdata0("Pending",questcount.Pending),
                new Dialogdata0("Finalized",questcount.Finalized),
                new Dialogdata0("Total",questcount.Total),
                new Dialogdata0("My Pending",questcount.MyPending),
                new Dialogdata0("My Finalized",questcount.MyFinalized),
                new Dialogdata0("Owner", questcount.Owner),
                new Dialogdata0("Newest",questcount.Newest),
            };

            this.ItemsSource = lst;
			}
			catch(Exception ex){
				
			}
        }
    }

    public class Dialogdata0
    {
        public string Category { get; set; }
        public string Details { get; set; }
        public Dialogdata0(String category, string detail)
        {
            Category = category;
            Details = detail;
        }
    }

    public class QuestCountData0
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