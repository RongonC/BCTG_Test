using Acr.UserDialogs;
using DataServiceBus.OfflineHelper.DataTypes;
using DataServiceBus.OfflineHelper.DataTypes.Cases;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Entity;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json.Linq;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.CustomControls;
using StemmonsMobile.DataTypes.DataType.Default;
using StemmonsMobile.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.LoginProcess
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage(InstanceList temp)
        {
            InitializeComponent();
            LoginButton.TextColor = Color.FromHex("1D9FEC");
            txt_uname.Text = "";
            txt_pwd.Text = "";

            DataServiceBus.OnlineHelper.DataTypes.Constants.Baseurl = temp.InstanceUrl;
            Application.Current.Properties["Baseurl"] = DataServiceBus.OnlineHelper.DataTypes.Constants.Baseurl;

            Functions.InstanceName = temp.InstanceName;
            Application.Current.Properties["InstanceName"] = Functions.InstanceName;

            Functions.Selected_Instance = temp.InstanceID;
            Application.Current.Properties["Selected_Instance"] = Functions.Selected_Instance;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //Task.Run(() =>
            //{
            //    App.GetBaseURLFromSQLServer();
            //});

            //if (Functions.IsPWDRemember && Functions.IsLogin)
            //{
            //    App.IsLoginCall = true;
            //    if (App.IsPropertyPage)
            //        await Navigation.PushAsync(new PropertyLandingPage());
            //    else
            //        await Navigation.PushAsync(new LandingPage());
            //}
            txt_uname.Focus();
        }

        async void BackButton_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
        async void Editbutton_Clicked(object sender, System.EventArgs e)
        {
            var list = await DBHelper.GetInstanceListByID(Functions.Selected_Instance, App.DBPath);

            await Navigation.PushAsync(new CreateNewInstance
            {
                BindingContext = list as InstanceList
            });
        }

        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_uname.Text))
            {
                txt_uname.Text = txt_uname.Text.ToLower();
            }
        }
        private void Btn_remember_Clicked(object sender, EventArgs e)
        {
            FileImageSource img = new FileImageSource();

            if (btn_remember.Image.File.Contains("Checked"))
            {
                img.File = "Assets/Unchecked.png";
                btn_remember.Image = img;
            }
            else
            {
                img.File = "Assets/Checked.png";
                btn_remember.Image = img;
            }
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            Functions.ShowOverlayView_StackLayout(overlay, true, Main_Stack);

            if (Functions.CheckInternetWithAlert())
            {
                Boolean IsLoginSuccess = false;
                if (!string.IsNullOrEmpty(txt_uname.Text) && !string.IsNullOrEmpty(txt_pwd.Text))
                {
                    try
                    {
                        JObject val = null;

                        await Task.Run(() =>
                        {
                            val = DefaultAPIMethod.LoginAuthenticate(txt_uname.Text, txt_pwd.Text);
                        });

                        if (val != null)
                        {
                            IsLoginSuccess = (Boolean)val.GetValue("Success");
                            if (IsLoginSuccess)
                            {
                                string[] userInfo = txt_uname.Text.Split("\\".ToCharArray());
                                string userDomain = userInfo[0].ToLower();
                                Functions.UserName = userInfo[1].ToLower();

                                Functions.HasTeam = (Boolean)val.GetValue("HasTeam");

                                Application.Current.Properties["HasTeam"] = Functions.HasTeam;

                                Functions.UserFullName = val.GetValue("result").ToString();

                                INSTANCE_USER_ASSOC _INSTANCE_USER_ASSOC = new INSTANCE_USER_ASSOC();
                                _INSTANCE_USER_ASSOC.INSTANCE_USER_ASSOC_ID = default(int);
                                _INSTANCE_USER_ASSOC.HOME_SCREEN_INFO = "";
                                _INSTANCE_USER_ASSOC.USER = Functions.UserName;
                                _INSTANCE_USER_ASSOC.INSTANCE_ID = Functions.Selected_Instance;

                                Task<int> t = DBHelper.Save_InstanceUserAssoc(_INSTANCE_USER_ASSOC, App.DBPath);
                                t.Wait();

                                ConstantsSync.INSTANCE_USER_ASSOC_ID = t.Result;

                                await Task.Run(() =>
                                {
                                    App.DownloadUserPicture();
                                });

                                if (ConstantsSync.INSTANCE_USER_ASSOC_ID > 1)
                                {
                                    Application.Current.Properties["AppStartCount"] = 1;
                                    Functions.AppStartCount = 1;
                                }

                                if (btn_remember.Image.File.Contains("Checked"))
                                {
                                    Functions.IsPWDRemember = true;
                                    Application.Current.Properties["IsPWDRemember"] = true;
                                }
                                else
                                {
                                    Functions.IsPWDRemember = false;
                                    Application.Current.Properties["IsPWDRemember"] = false;
                                }

                                Application.Current.Properties["UserName"] = Functions.UserName;
                                Application.Current.Properties["UserFullName"] = Functions.UserFullName;
                                Application.Current.Properties["IsLogin"] = true;
                                Application.Current.Properties["UserLoginName"] = txt_uname.Text;
                                Application.Current.Properties["UserPassword"] = txt_pwd.Text;
                            }
                            else
                            {
                                Application.Current.Properties["UserName"] = "";
                                Application.Current.Properties["UserFullName"] = "";
                                Application.Current.Properties["IsLogin"] = false;
                                Application.Current.Properties["UserLoginName"] = "";
                                Application.Current.Properties["UserPassword"] = "";
                                Application.Current.Properties["HasTeam"] = false;
                                DisplayAlert("Error!", val.GetValue("Message").ToString(), "Ok");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(txt_uname.Text))
                    {
                        txt_uname.Focus();
                        DisplayAlert("Required!", "Please, Enter UserName.", "Ok");
                    }
                    else if (string.IsNullOrEmpty(txt_pwd.Text))
                    {
                        txt_pwd.Focus();
                        DisplayAlert("Required!", "Please, Enter Password.", "Ok");
                    }
                }

                if (IsLoginSuccess)
                {

                    App.IsLoginCall = true;
                    if (App.IsCustomLandingPage)
                        await Navigation.PushAsync(new CustomlandingPage());
                    else
                        await Navigation.PushAsync(new LandingPage());
                }
            }
            Functions.ShowOverlayView_StackLayout(overlay, false, Main_Stack);
        }

        private void txt_uname_Completed(object sender, EventArgs e)
        {
            txt_pwd.Focus();
        }

        private void txt_pwd_Completed(object sender, EventArgs e)
        {
            LoginButton_Clicked(sender, e);
        }
    }
}
