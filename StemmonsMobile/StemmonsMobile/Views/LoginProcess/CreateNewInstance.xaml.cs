using DataServiceBus;
using DataServiceBus.OfflineHelper.DataTypes;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.LoginProcess
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateNewInstance : ContentPage
    {
        public CreateNewInstance()
        {
            InitializeComponent();
            Createbtn.TextColor = Color.FromHex("1D9FEC");

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            txt_instanceurl.Text = "BoxerProperty Property";
            txt_instancename.Text = "http://home.boxerproperty.com/mobileapi";
            if (Functions.Selected_Instance == 0)
            {
                Title = "Create New Instance";
                Createbtn.Text = "Create";
            }
            else
            {
                Title = "Edit Instance";
                Createbtn.Text = "Update";
            }
            txt_instancename.Focus();
        }
        async void BackButton_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_instanceurl.Text))
            {
                txt_instanceurl.Text = txt_instanceurl.Text.ToLower();
            }
        }

        string GetINstaceList()
        {
            try
            {
                string resultedValueForService = string.Empty;

                if (txt_instanceurl.Text.Trim() != "")
                {
                    string orignalServiceURL = this.txt_instanceurl.Text.Replace("http://", "").Replace("https://", "");

                    List<string> posibilities = new List<string>();

                    string[] prefixes =
                        {
                        "","http://","https://","http://home.","https://home.","https://stems.","http://stems.","http://stemmons.","https://stemmons.", "http://central.", "https://central."
                    };

                    string[] suffexes =
                     {
                        "","/","/mobileapi/token","mobileapi/token","/token","token","/mobileapitest/token","mobileapitest/token"
                    };

                    foreach (string prefix in prefixes)
                    {
                        posibilities.Add(prefix + orignalServiceURL);
                    }
                    foreach (string suffex in suffexes)
                    {
                        posibilities.Add(orignalServiceURL + suffex);
                    }
                    foreach (string prefix in prefixes)
                    {
                        foreach (string suffex in suffexes)
                        {
                            posibilities.Add(prefix + orignalServiceURL + suffex);
                        }
                    }

                    posibilities.RemoveAll(v => v.ToLower().Contains("/mobileapi/mobileapi/token"));
                    try
                    {
                        posibilities.RemoveAll(v => v.ToLower().Contains("/mobileapitest/mobileapitest/token"));
                    }
                    catch (Exception)
                    {
                    }
                    var te = posibilities.FindAll(v => v.ToLower().Contains("/mobileapi/token") || v.ToLower().Contains("/mobileapitest/token"));
                    foreach (string item in te)
                    {
                        try
                        {
                            // Validate Webpage
                            if (!string.IsNullOrEmpty(CheckUserHasAccess(item)))
                            {
                                return item;
                            }
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            catch { }
            finally
            {
            }
            return "";
        }

        private string CheckUserHasAccess(string InstanceURL)
        {
            try
            {
                return MobileAPIMethods.RequestToken(InstanceURL);
            }
            catch (WebException ex)
            {
            }
            return "";
        }

        private async void Createbtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                string I_Name = txt_instancename.Text;
                string I_URL = string.Empty;///*txt_instanceurl.Text;*/ GetINstaceList();
                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                await Task.Run(() =>
                {
                    I_URL = GetINstaceList();
                });

                Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                if (!string.IsNullOrEmpty(I_Name) && !string.IsNullOrEmpty(I_URL))
                {
                    if (Uri.IsWellFormedUriString(I_URL, UriKind.Absolute))
                    {
                        I_URL = I_URL.Split(new string[] { "/token" }, StringSplitOptions.None)[0]; //(0, I_URL.LastIndexOf('/token'));

                        InstanceList insta = new InstanceList
                        {
                            InstanceName = I_Name,
                            InstanceUrl = I_URL,
                            InstanceID = Functions.Selected_Instance
                        };

                        var s = await DBHelper.SaveInstance(insta, App.DBPath);
                        if (s == 1)
                        {
                            //DataServiceBus.OnlineHelper.DataTypes.Constants.Baseurl = I_URL;
                            DisplayAlert("Success", "Database Operation Succcess.\n URL: " + I_URL, "Ok");
                            Navigation.PopAsync();
                        }
                        else
                        {
                            DisplayAlert("Error!", "Database Operation failed.", "Ok");
                        }
                    }
                    else
                    {
                        DisplayAlert("Error!", "Enter Valid URL.", "Ok");
                    }
                }
                else
                {
                    DisplayAlert("Error!", "Enter Valid Data.", "Ok");
                }
            }
            catch (Exception)
            {
            }

        }

        private void txt_instancename_Completed(object sender, EventArgs e)
        {
            txt_instanceurl.Focus();
        }

        private void txt_instanceurl_Completed(object sender, EventArgs e)
        {
            Createbtn_Clicked(sender, e);
        }
    }
}
