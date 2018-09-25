using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Default;
using StemmonsMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.Standards
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TopicPage : ContentPage
    {
        Topic N_topic = new Topic();

        public TopicPage(Topic topic)
        {
            InitializeComponent();
            App.SetConnectionFlag();
            this.Title = topic.NAME;
            N_topic = topic;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                string content = @"<html><body>";
                content += Recursion(N_topic);
                content += @"</body></html>";
                var htmlWebViewSource = new HtmlWebViewSource { Html = content };
                this.webView.Source = htmlWebViewSource;
                _content = string.Empty;
            }
            catch (Exception e)
            {
            }
        }

        static string _content = "";
        public static string Recursion(Topic topic)
        {
            try
            {
                _content += "<p><b><u><font size=\"5\">" + topic.NAME + "</font></u></b></p>";
                _content += "<p>" + topic.METADATA_DESC + "</p> ";

                _content += "<p>" + GetBasePathWithURL(topic.METADATA_CONTENT) + "</p> ";

                foreach (var item in topic.Subtopics)
                {
                    Recursion(item);
                }
            }
            catch (Exception)
            {
            }

            return _content;
        }

        // Replace URL download text with base URL to display Images
        private static string GetBasePathWithURL(string InPutString)
        {
            try
            {
                string result = string.Empty;

                string sFilePath1 = "src=\"Download.aspx?FileID=";
                string sFilePath1_new = "src=\"" + App.StandardImgURL + "/Download.aspx?FileID=";

                string sFilePath2 = "src=\"download.aspx?FileID=";
                string sFilePath2_new = "src=\"" + App.StandardImgURL + "/download.aspx?FileID=";

                string sFilePath3 = "src=\"Download.aspx?FileID=";
                string sFilePath3_new = "src=\"" + App.StandardImgURL + "/Download.aspx?FileID=";

                string sFilePath4 = "src=\"download.aspx?fileid=";
                string sFilePath4_new = "src=\"" + App.StandardImgURL + "/download.aspx?fileid=";

                string sFilePath5 = "src=\"/download.aspx?fileid=";
                string sFilePath5_new = "src=\"" + App.StandardImgURL + "/download.aspx?fileid=";

                string sFilePath6 = "src=\"/Download.aspx?fileid=";
                string sFilePath6_new = "src=\"" + App.StandardImgURL + "/download.aspx?fileid=";

                string sFilePath7 = "src=\"/Download.aspx?FileID=";
                string sFilePath7_new = "src=\"" + App.StandardImgURL + "/download.aspx?fileid=";

                string sFilePath8 = "src=\"../Images/mswordIcon.jpg";
                string sFilePath8_new = "src=\"" + App.StandardImgURL + "/Images/mswordIcon.jpg";

                string sFilePath9 = "src=\"../Images/pdfIcon.jpg";
                string sFilePath9_new = "src=\"" + App.StandardImgURL + "/Images/pdfIcon.jpg";

                string sFilePath10 = "src=\"../Images/msExcel.jpg";
                string sFilePath10_new = "src=\"" + App.StandardImgURL + "/Images/msExcel.jpg";


                result = InPutString.Replace(sFilePath1, sFilePath1_new).Replace(sFilePath2, sFilePath2_new).Replace(sFilePath3, sFilePath3_new).Replace(sFilePath4, sFilePath4_new).Replace(sFilePath5, sFilePath5_new).Replace(sFilePath6, sFilePath6_new).Replace(sFilePath7, sFilePath7_new).Replace(sFilePath8, sFilePath8_new).Replace(sFilePath9, sFilePath9_new).Replace(sFilePath10, sFilePath10_new);

                return result;
            }
            catch (Exception)
            {
                return InPutString;
            }
        }

        private async void Topics_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}