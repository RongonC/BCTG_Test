using System;
using System.Linq;
using Xamarin.Forms;
using Newtonsoft.Json;
using Xamarin.Forms.Xaml;
using System.ComponentModel;
using StemmonsMobile.Models;
using System.Collections.Generic;
using StemmonsMobile.Commonfiles;
using System.Collections.ObjectModel;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Standards;
using StemmonsMobile.DataTypes.DataType.Standards.DataTypes;
using System.Threading.Tasks;

namespace StemmonsMobile.Views.Standards
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookContentsPage : ContentPage, INotifyPropertyChanged
    {
        public static ObservableCollection<Topic> Tlist2 = new ObservableCollection<Topic>();

        Grp_StandardDetails SelectedNodes;

        public BookContentsPage(Grp_StandardDetails _SelectedNodes)
        {
            InitializeComponent();
            Tlist2.Clear();
            SelectedNodes = _SelectedNodes;
            Title = SelectedNodes.APP_NAME;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            App.SetConnectionFlag();
            Tlist2.Clear();

            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);

            standardsBookView Bookviewresponse = null;
            try
            {
                await Task.Run(() =>
                {
                    Bookviewresponse = StandardsSyncAPIMethods.BookViewOnlineOffline(App.Isonline, Convert.ToInt32(SelectedNodes.APP_ID), ConstantsSync.StandardInstance, Functions.UserName, App.DBPath).Result;
                });
            }
            catch (Exception e)
            {
            }

            if (Bookviewresponse?.BookView?.Count > 0)
            {
                var view = JsonConvert.SerializeObject(Bookviewresponse.BookView);
                List<Topic> Tlist = new List<Topic>();
                Tlist = (JsonConvert.DeserializeObject<List<Topic>>(view.ToString()));
                if (Tlist.Count() > 0)
                {

                    Topic Final = new Topic();

                    Final = ArrangeTree(Tlist);

                    //for (int i = 0; i < Tlist.Count; i++)
                    //{

                    //    if (Tlist[i].PARENT_LEVEL != "1")
                    //    {
                    //        if (Tlist[i].pARENT_META_DATA_IDField == Final.APP_ASSOC_META_DATA_ID)
                    //            Final.Subtopics.AddRange(new[] { Tlist[i] });
                    //        else
                    //        {
                    //            for (int j = 0; j < Final.Subtopics.Count; j++)
                    //            {
                    //                //BuildTreeTopic(Tlist[i], ref Final);
                    //                if (Tlist[i].pARENT_META_DATA_IDField == Final.Subtopics[j].APP_ASSOC_META_DATA_ID)
                    //                    Final.Subtopics[j].Subtopics.AddRange(new[] { Tlist[i] });
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        Final = Tlist[i];
                    //    }

                    //}

                    Tlist2.Add(Final);

                    Standardbooktreeview.ItemsSource = Tlist2;
                }
                else
                {
                    DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                }
            }
            else
            {
                DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
            }

            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }


        private Topic ArrangeTree(List<Topic> Tlist)
        {
            if (Tlist.Count < 0)
            {
                return new Topic();
            }

            Topic tp = new Topic();
            //

            var tpFirst = Tlist.Where(t => t.PARENT_META_DATA_ID == "0").FirstOrDefault();
            tp = tpFirst;
            var childtp = Tlist.Where(t => t.PARENT_META_DATA_ID == tp.APP_ASSOC_META_DATA_ID).ToList();
            ArrangeTree(Tlist, ref childtp);
            tp.Subtopics.AddRange(childtp);

            // 1,2,3,4 child

            //foreach (var subchild in childtp)
            //{
            //    var subchildtp = Tlist.Where(t => t.pARENT_LEVELField == subchild.APP_ASSOC_META_DATA_ID);
            //    tp.Subtopics.AddRange(childtp);
            //}            

            //for (int i = 0; i <= Tlist.Count - 1; i++)
            //{
            //    int iAPP_ASSOC_META_DATA_ID = int.Parse(Tlist[i].APP_ASSOC_META_DATA_ID);
            //    var childtp = Tlist.Where(t => int.Parse(t.pARENT_LEVELField) == iAPP_ASSOC_META_DATA_ID).ToList();
            //    Tlist[i].Subtopics.AddRange(childtp);
            //}
            return tp;
        }



        private void ArrangeTree(List<Topic> Tlist, ref List<Topic> Childlist)
        {
            for (int j = 0; j <= Childlist.Count - 1; j++)
            {
                int iAPP_ASSOC_META_DATA_ID = int.Parse(Childlist[j].APP_ASSOC_META_DATA_ID);
                var childtp = Tlist.Where(t => int.Parse(t.PARENT_META_DATA_ID) == iAPP_ASSOC_META_DATA_ID).ToList();
                Childlist[j].Subtopics.AddRange(childtp);
                List<Topic> Childitem = new List<Topic>();
                Childitem = Childlist[j].Subtopics;
                ArrangeTree(Tlist, ref Childitem);
                Childlist[j].Subtopics = Childitem;
            }
        }

        public void BuildTreeTopic(Topic ItemtoAdd, Topic Mainlists)
        {

            // if (Tlist[i].pARENT_META_DATA_IDField == Mainlists.Subtopics[j].APP_ASSOC_META_DATA_ID)

            foreach (var item in Mainlists.Subtopics)
            {
                BuildTreeTopic(ItemtoAdd, item);
            }
        }

        private void Topic_Changed(object sender, EventArgs e)
        {
            var treeView = ((TreeView.TreeView)sender).SelectedItem;
            this.Navigation.PushAsync(new TopicPage(treeView.BindingContext as Topic));
        }

        private void Icon_book_Clicked(object sender, EventArgs e)
        {
            if (Tlist2.Count > 0)
                this.Navigation.PushAsync(new TopicPage(Tlist2[0]));
        }
        public void Recursion(Topic topic, string searchWord)
        {
            Topic Final = new Topic();
            if (topic.NAME.Contains(searchWord))
                Final = topic;

            foreach (var item in topic.Subtopics)
            {
                if (item.PARENT_LEVEL != "1")
                {
                    if (item.pARENT_META_DATA_IDField == topic.APP_ASSOC_META_DATA_ID)
                    {
                        if (item.NAME.Contains(searchWord))
                            Final.Subtopics.AddRange(new[] { item });
                    }
                    else
                    {
                        for (int j = 0; j < topic.Subtopics.Count; j++)
                        {
                            if (item.pARENT_META_DATA_IDField == topic.Subtopics[j].APP_ASSOC_META_DATA_ID)
                            {
                                if (item.NAME.Contains(searchWord))
                                    Final.Subtopics[j].Subtopics.AddRange(new[] { item });
                            }
                        }
                    }
                }
                else
                {
                    if (item.NAME.Contains(searchWord))
                        Final = item;
                }

                Recursion(item, searchWord);
            }
            Search_List.Add(Final);
        }
        ObservableCollection<Topic> Search_List = new ObservableCollection<Topic>();

        private void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Still Testing is in Progress Having issue in compare and Fill Nested List of Books
            // Search_List.Clear();
            try
            {
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    Standardbooktreeview.ItemsSource = Tlist2;
                }
                else
                {
                    List<Topic> Details_List = new List<Topic>();
                    int i = 0;
                    foreach (var item in Tlist2)
                    {
                        Topic Final = new Topic();
                        for (i = 0; i < Tlist2.Count; i++)
                        {
                            if (Tlist2[i].PARENT_LEVEL != "1")
                            {
                                if (Tlist2[i].pARENT_META_DATA_IDField == Final.APP_ASSOC_META_DATA_ID)
                                {
                                    if (Tlist2[i].NAME.Contains(e.NewTextValue))
                                        Final.Subtopics.AddRange(new[] { Tlist2[i] });
                                }
                                else
                                {
                                    for (int j = 0; j < Final.Subtopics.Count; j++)
                                    {
                                        if (Tlist2[i].pARENT_META_DATA_IDField == Final.Subtopics[j].APP_ASSOC_META_DATA_ID)
                                        {
                                            if (Tlist2[i].NAME.Contains(e.NewTextValue))
                                                Final.Subtopics[j].Subtopics.AddRange(new[] { Tlist2[i] });
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (Tlist2[i].NAME.Contains(e.NewTextValue))
                                    Final = Tlist2[i];
                            }

                        }
                        Search_List.Add(Final);

                        Recursion(item, e.NewTextValue);
                    }
                    Standardbooktreeview.ItemsSource = Search_List.Where(v => v.NAME != null).ToList();
                }
            }
            catch (Exception)
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