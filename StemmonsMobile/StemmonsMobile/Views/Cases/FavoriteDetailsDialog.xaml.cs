using DataServiceBus.OfflineHelper.DataTypes.Cases;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OnlineHelper.DataTypes;
using StemmonsMobile.Commonfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static StemmonsMobile.DataTypes.DataType.Cases.GetFavoriteResponse;

namespace StemmonsMobile.Views.Cases
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoriteDetailsDialog : ContentPage
    {
        string FavoriteID;
        List<GetFavorite> list_favorite = new List<GetFavorite>();

        public FavoriteDetailsDialog()
        {
            InitializeComponent();
        }

        public FavoriteDetailsDialog(string FavoriteId) : this()
        {
            try
            {
                FavoriteID = FavoriteId;
                var result_fav = CasesSyncAPIMethods.GetFavorite(App.Isonline, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                if (result_fav.Result != null && result_fav.Result.ToString() != "[]")
                {
                    GetFavorite getfavorite;
                    if (result_fav.Result.Count > 0)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
