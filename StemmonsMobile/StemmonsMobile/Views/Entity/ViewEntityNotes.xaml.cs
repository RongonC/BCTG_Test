using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Entity;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.Entity
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewEntityNotes : ContentPage
    {
        List<Entity_Notes> Entity_NotesLists = new List<Entity_Notes>();

        public ViewEntityNotes(string _entityID, string _entityTypeID)
        {
            InitializeComponent();
            LoadNotes(_entityID, _entityTypeID);
        }

        public async void LoadNotes(string EntityID, string EntityTypeID)
        {
            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
            ObservableCollection<EntityNotesGroup> NotesGroups = new ObservableCollection<EntityNotesGroup>();

            try
            {
                await Task.Run(async () =>
                {
                    try
                    {
                        Entity_NotesLists = await EntitySyncAPIMethods.GetNotes(App.Isonline, EntityID, Functions.UserName, EntityTypeID.ToString(), this.Title, App.DBPath);
                    }
                    catch (Exception)
                    {
                    }
                });

                ObservableCollection<EntityNotesGroup> Temp = new ObservableCollection<EntityNotesGroup>();
                if (Entity_NotesLists != null)
                {
                    for (int i = 0; i < Entity_NotesLists.Count; i++)
                    {
                        var st = Convert.ToDateTime(CommonConstants.DateFormatStringToString(Entity_NotesLists[i].CreatedDatetime));
                        Temp.Add(new EntityNotesGroup("", st.Date.ToString("d"), Entity_NotesLists[i].CreatedBy)
                        {
                            new Entity_Notes { Note = Entity_NotesLists[i].Note }
                        });
                    }

                    foreach (var item in Temp)
                    {
                        if (item.FirstOrDefault().Note.Contains("<img"))
                        {
                            item.FirstOrDefault().ImageVisible = true;
                            item.FirstOrDefault().LabelVisible = true;
                            item.FirstOrDefault().htmlNote = item.FirstOrDefault().Note;
                            item.FirstOrDefault().ImageURL = App.EntityImgURL + "/" + Functions.HTMLToText(item.FirstOrDefault().Note.Replace("'", "\"").Split('\"')[1]);
                            item.FirstOrDefault().Note = Functions.HTMLToText(item.FirstOrDefault().Note);
                        }
                        else
                        {
                            item.FirstOrDefault().ImageVisible = false;
                            item.FirstOrDefault().LabelVisible = true;
                            item.FirstOrDefault().htmlNote = item.FirstOrDefault().Note;
                            item.FirstOrDefault().Note = Functions.HTMLToText(item.FirstOrDefault().Note);
                        }
                        NotesGroups.Add(item);
                    }
                }
                else
                {
                    await DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                    await Navigation.PopAsync();
                }
                gridEntitynotes.ItemsSource = NotesGroups;
            }
            catch (Exception)
            {
            }

            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }

        private async void GridEntitynotes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var notes = gridEntitynotes.SelectedItem as Entity_Notes;
                if (notes.ImageVisible)
                {
                    await Navigation.PushAsync(new ViewAttachment
                    (notes.ImageURL));
                }
                gridEntitynotes.SelectedItem = null;
            }
            catch (Exception ex)
            {
            }
        }
    }
}