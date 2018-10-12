using DataServiceBus.OfflineHelper.DataTypes.Cases;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Cases;
using StemmonsMobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.Cases
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewcaseNotes : ContentPage
    {
        private ObservableCollection<CasesNotesGroup> CasesnotesGroups = new ObservableCollection<CasesNotesGroup>();
        public ViewcaseNotes(bool Onlineflag, string CaseID, string Casetypeid)
        {
            InitializeComponent();

            LoadCasenotes(Onlineflag, CaseID, Casetypeid);
        }

        private async void LoadCasenotes(bool Onlineflag, string CaseID, string Casetypeid)
        {
            try
            {
                //gridCasesnotes.ItemsSource = null;
                //CasesnotesGroups.Clear();
                //Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                //List<GetCaseNotesResponse.NoteData> Noteslist = new List<GetCaseNotesResponse.NoteData>();
                //await Task.Run(() =>
                //{
                //    Task<List<GetCaseNotesResponse.NoteData>> NotesResponse = CasesSyncAPIMethods.GetCaseNotes(Onlineflag, CaseID, Casetypeid, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                //    NotesResponse.Wait();
                //    Noteslist = NotesResponse?.Result;
                //});

                //ObservableCollection<CasesNotesGroup> Temp = new ObservableCollection<CasesNotesGroup>();
                //if (Noteslist?.Count > 0)
                //{
                //    for (int i = 0; i < Noteslist.Count; i++)
                //    {
                //        CasesNotesGroup grp = new CasesNotesGroup("", Convert.ToString(Noteslist[i].CreatedDateTime), Noteslist[i]?.CreatedByUser == null ? Functions.UserFullName : Noteslist[i]?.CreatedByUser?.DisplayName)
                //            {
                //                new GetCaseNotesResponse.NoteData
                //                {
                //                    Note = Noteslist[i].Note
                //                }
                //            };
                //        Temp.Add(grp);
                //    }

                //    foreach (var item in Temp)
                //    {
                //        if (item.FirstOrDefault().Note.Contains("<img"))
                //        {
                //            item.FirstOrDefault().ImageVisible = true;
                //            item.FirstOrDefault().LabelVisible = true;
                //            item.FirstOrDefault().HtmlNote = item.FirstOrDefault().Note;
                //            item.FirstOrDefault().ImageURL = App.CasesImgURL + "/" + Functions.HTMLToText(item.FirstOrDefault().Note.Replace("'", "\"").Split('\"')[1]);
                //            item.FirstOrDefault().Note = Functions.HTMLToText(item.FirstOrDefault().Note);
                //        }
                //        else
                //        {
                //            item.FirstOrDefault().ImageVisible = false;
                //            item.FirstOrDefault().LabelVisible = true;
                //            item.FirstOrDefault().HtmlNote = item.FirstOrDefault().Note;
                //            item.FirstOrDefault().Note = Functions.HTMLToText(item.FirstOrDefault().Note);
                //        }

                //        CasesnotesGroups.Add(item);
                //    }

                //    gridCasesnotes.ItemsSource = CasesnotesGroups;
                //}
                //else
                //{
                //    await DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                //    await Navigation.PopAsync();
                //}
            }
            catch (Exception ex)
            {
            }

            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }

        private async void GridCasesnotes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                string url = string.Empty;
                var d = DataServiceBus.OnlineHelper.DataTypes.Constants.Baseurl.Split(new string[] { "/mobileapi" }, StringSplitOptions.None);
                url = d[0];

                var notes = gridCasesnotes.SelectedItem as GetCaseNotesResponse.NoteData;

                if (notes.ImageVisible)
                {
                    await Navigation.PushAsync(new ViewAttachment
                    (notes.ImageURL));
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}