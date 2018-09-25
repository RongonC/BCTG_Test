using System;
using System.ComponentModel; using Xamarin.Forms; using System.Windows.Input; using System.Globalization; using System.Collections.Generic; using System.Text.RegularExpressions;
using StemmonsMobile.Views.Cases;
using DataServiceBus.OnlineHelper.DataTypes;
using static StemmonsMobile.DataTypes.DataType.Quest.GetSecurityDataResponse;
using Newtonsoft.Json;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.Views.CreateQuestForm;
using StemmonsMobile.DataTypes.DataType.Cases;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.Views.Entity;
using DataServiceBus.OfflineHelper.DataTypes;
using Plugin.Connectivity;
using StemmonsMobile.DataTypes.DataType.Default;
using System.Linq;

namespace StemmonsMobile
{
    public class HtmlLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var formatted = new FormattedString();

            foreach (var item in ProcessString((string)value))
                formatted.Spans.Add(CreateSpan(item));

            return formatted;
        }

        private Span CreateSpan(StringSection section)
        {
            var label = new Span()
            {
                Text = section.Text
            };

            if (!string.IsNullOrEmpty(section.Link))
            {
                label.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = _navigationCommand,
                    CommandParameter = section.Link
                });




                label.TextColor = Color.Blue;
                // Underline coming soon from https://github.com/xamarin/Xamarin.Forms/pull/2221
                // Currently available in Nightly builds if you wanted to try, it does work :)
                // As of 2018-07-22. But not avail in 3.2.0-pre1.
                //label.TextDecorations = TextDecorations.Underline;
            }

            return label;
        }

        public IList<StringSection> ProcessString(string rawText)
        {
            //         // Remove HEAD tag
            //         rawText = Regex.Replace(rawText, "<head.*?</head>", ""
            //                             , RegexOptions.IgnoreCase | RegexOptions.Singleline);

            //         // Remove any JavaScript
            //         rawText = Regex.Replace(rawText, "<script.*?</script>", ""
            //           , RegexOptions.IgnoreCase | RegexOptions.Singleline);

            //         string[] OldWords = {"&nbsp;", "&amp;", "&quot;", "&lt;",
            //"&gt;", "&reg;", "&copy;", "&bull;", "&trade;", "&#39;"};
            //         string[] NewWords = { " ", "&", "\"", "<", ">", "Â®", "Â©", "â€¢", "â„¢", "'" };
            //         for (int i = 0; i < OldWords.Length; i++)
            //         {
            //             rawText.Replace(OldWords[i], NewWords[i]);
            //         }

            //         //rawText.Replace("\astart", "<a");
            //         //rawText.Replace("\aEnd", "</a>");
            //         //rawText.Replace("&amp;", "&");
            //         //rawText.Replace("&nbsp;", " ");

            //         rawText = rawText.Replace("<br>", Environment.NewLine);
            //         rawText = rawText.Replace("<br/>", Environment.NewLine);
            //         rawText = rawText.Replace("<br />", Environment.NewLine);
            //         rawText = rawText.Replace("<br ", Environment.NewLine);

            //         rawText = rawText.Replace("\n", Environment.NewLine);

            //         rawText = rawText.Replace("<p>", Environment.NewLine);
            //         rawText = rawText.Replace("<p ", Environment.NewLine);

            //         //rawText.Replace("<a", "\astart");
            //         //rawText.Replace("</a>", "\aEnd");

            //         //var tty = Functions.HTMLToText(rawText);

            rawText = rawText.Replace("<p>", Environment.NewLine);
            rawText = rawText.Replace("&nbsp;", " ");


            List<StringSection> list = new List<StringSection>();
            const string spanPattern = @"(<a.*?>.*?</a>)";
            const string spanPattern1 = @"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?";

            MatchCollection collection1 = Regex.Matches(rawText, spanPattern1, RegexOptions.Singleline);


            MatchCollection collection = Regex.Matches(rawText, spanPattern, RegexOptions.Singleline);


            // 1.
            // Find all matches in file.
            MatchCollection m1 = Regex.Matches(rawText, @"(<a.*?>.*?</a>)",
                RegexOptions.Singleline);

            // 2.
            // Loop over each match.
            foreach (Match m in m1)
            {
                string value = m.Groups[1].Value;
                StringSection i = new StringSection();

                // 3.
                // Get href attribute.
                Match m2 = Regex.Match(value, @"href=\""(.*?)\""",
                RegexOptions.Singleline);
                if (m2.Success)
                {
                    i.Link = m2.Groups[1].Value;
                }

                // 4.
                // Remove inner tags from text.
                string ta = Regex.Replace(value, @"\s*<.*?>\s*", "",
                RegexOptions.Singleline);
                i.Text = ta;

                list.Add(i);

            }

            var sections = new List<StringSection>();

            var lastIndex = 0;

            foreach (Match item in collection)
            {
                var foundText = item.Value;
                sections.Add(new StringSection() { Text = rawText.Substring(lastIndex, item.Index - lastIndex) });
                lastIndex = item.Index + item.Length;

                MatchCollection mC2 = Regex.Matches(item.Value, @"(<a.*?>.*?</a>)",
                RegexOptions.Singleline);
                // Get HTML href 
                var html = new StringSection()
                {
                    Link = Regex.Match(item.Value, spanPattern1).Value,
                    Text = Regex.Replace(item.Value, "<.*?>", string.Empty)
                };

                foreach (Match m in mC2)
                {
                    string value = m.Groups[1].Value;
                    StringSection i = new StringSection();

                    // 3.
                    // Get href attribute.
                    Match m2 = Regex.Match(value, @"href=\""(.*?)\""",
                    RegexOptions.Singleline);
                    if (m2.Success)
                    {
                        html.Link = m2.Groups[1].Value;
                    }
                }

                sections.Add(html);
            }

            foreach (Match item in collection1)
            {
                var foundText = item.Value;
                sections.Add(new StringSection() { Text = rawText.Substring(lastIndex, item.Index - lastIndex) });
                lastIndex = item.Index + item.Length;

                // Get HTML href 
                var html = new StringSection()
                {
                    Link = Regex.Match(item.Value, spanPattern1).Value,
                    Text = Regex.Replace(item.Value, "<.*?>", string.Empty)
                };

                sections.Add(html);
            }


            sections.Add(new StringSection() { Text = Functions.HTMLToText(rawText.Substring(lastIndex)) });
            //var nHTML = Functions.HTMLToText(rawText.Substring(lastIndex));
            //list.Add(new StringSection() { Text = nHTML });

            return sections;
        }

        public class StringSection
        {
            public string Text { get; set; }
            public string Link { get; set; }
        }

        private ICommand _navigationCommand = new Command<string>(async (url) =>
        {
            try
            {
                url = url.ToLower();
                string appname = string.Empty;
                int link_Nav_id = 0;
                string u = DataServiceBus.OnlineHelper.DataTypes.Constants.Baseurl.Split(new string[] { "/mobileapi" }, StringSplitOptions.None)[0] + "/";

                var tempU = DBHelper.UserScreenRetrive("SYSTEMCODES", App.DBPath, "SYSTEMCODES");

                var Check = JsonConvert.DeserializeObject<List<MobileBranding>>(tempU.ASSOC_FIELD_INFO);
                List<string> ls = new List<string>();
                foreach (var item in Check)
                {
                    MatchCollection collection = Regex.Matches(item.VALUE.ToLower(), @"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?", RegexOptions.Singleline);

                    foreach (Match itm in collection)
                    {
                        ls.Add(itm.Value);
                    }
                }

                bool isopenWeb = true;
                foreach (var item in ls)
                {
                    if (url.Contains(item.ToLower()))
                    {
                        isopenWeb = false;
                        break;
                    }
                    else
                    {
                        isopenWeb = true;
                    }
                }

                if (!isopenWeb)
                {
                    if (url.Contains("?caseid="))
                    {
                        appname = "ViewCases";
                        string tSTR = string.Empty;
                        var eqsplit = url.Split(new string[] { "caseid=" }, StringSplitOptions.None);
                        foreach (var ite in eqsplit[1].ToCharArray())
                        {
                            if (char.IsDigit(ite))
                            {
                                tSTR += ite;
                            }
                            else
                            {
                                break;
                            }
                        }
                        link_Nav_id = System.Convert.ToInt32(tSTR);
                    }
                    else if (url.Contains("?casetypeid=") && url.Contains("&listid="))
                    {
                        appname = "ViewCases";
                        string link_Nav_Typeid = "0";
                        string link_Nav_Listid = "0";
                        var eqsplit = url.Split(new string[] { "?casetypeid=" }, StringSplitOptions.None);
                        foreach (var ite in eqsplit[1].ToCharArray())
                        {
                            if (char.IsDigit(ite))
                            {
                                link_Nav_Typeid += ite;
                            }
                            else
                            {
                                break;
                            }
                        }

                        eqsplit = url.Split(new string[] { "&listid=" }, StringSplitOptions.None);
                        foreach (var ite in eqsplit[1].ToCharArray())
                        {
                            if (char.IsDigit(ite))
                            {
                                link_Nav_Listid += ite;
                            }
                            else
                            {
                                break;
                            }
                        }

                        var result = CasesAPIMethods.GetCaseIDByCaseTypeIDAndListID(link_Nav_Typeid, link_Nav_Listid);
                        var tm = result.GetValue("ResponseContent").ToString();
                        link_Nav_id = System.Convert.ToInt32(tm);
                    }
                    else if (url.Contains("?intiteminstancetranid="))
                    {
                        appname = "ViewQuest";
                        string Qstr = string.Empty;
                        var eqsplit = url.Split(new string[] { "?intiteminstancetranid=" }, StringSplitOptions.None);
                        foreach (var ite in eqsplit[1].ToCharArray())
                        {
                            if (char.IsDigit(ite))
                            {
                                Qstr += ite;
                            }
                            else
                            {
                                break;
                            }
                        }
                        link_Nav_id = System.Convert.ToInt32(Qstr);
                    }
                    else if (url.Contains("?entityid="))
                    {
                        appname = "ViewEntities";
                        string eSTR = string.Empty;
                        var eqsplit = url.Split(new string[] { "entityid=" }, StringSplitOptions.None);
                        foreach (var ite in eqsplit[1].ToCharArray())
                        {
                            if (char.IsDigit(ite))
                            {
                                eSTR += ite;
                            }
                            else
                                break;
                        }
                        link_Nav_id = System.Convert.ToInt32(eSTR);
                    }
                    else if (url.Contains("/newform.aspx?") && url.Contains("?areaid=") && url.Contains("&itemid="))
                    {
                        appname = "NewForm";
                        string aID = "0";
                        string itmID = "0";
                        var temp = url.Split(new string[] { "?areaid=" }, StringSplitOptions.None);
                        foreach (var ite in temp[1].ToCharArray())
                        {
                            if (char.IsDigit(ite))
                            {
                                aID += ite;
                            }
                            else
                            {
                                break;
                            }
                        }

                        temp = url.Split(new string[] { "&itemid=" }, StringSplitOptions.None);
                        foreach (var ite in temp[1].ToCharArray())
                        {
                            if (char.IsDigit(ite))
                            {
                                itmID += ite;
                            }
                            else
                            {
                                break;
                            }
                        }

                        await Application.Current.MainPage.Navigation.PushAsync(new NewQuestForm(System.Convert.ToInt32(itmID), System.Convert.ToInt32(aID).ToString()));
                    }
                    else if (url.Contains("/newcase.aspx?") && url.Contains("?casetypeid="))
                    {
                        string iD = "0";
                        int CASETYPEID = 0;
                        var eqsplit = url.Split(new string[] { "?casetypeid=" }, StringSplitOptions.None);
                        foreach (var ite in eqsplit[1].ToCharArray())
                        {
                            if (char.IsDigit(ite))
                            {
                                iD += ite;
                            }
                            else
                            {
                                break;
                            }
                        }
                        CASETYPEID = System.Convert.ToInt32(iD);
                        await Application.Current.MainPage.Navigation.PushAsync(new NewCase("0", CASETYPEID.ToString(), "Create Case"));
                    }
                    else if (url.Contains("/entitycreate.aspx?") && url.Contains("?entitytypeid="))
                    {
                        string entitytypeid = "0";
                        var eqsplit = url.Split(new string[] { "?entitytypeid=" }, StringSplitOptions.None);
                        foreach (var ite in eqsplit[1].ToCharArray())
                        {
                            if (char.IsDigit(ite))
                            {
                                entitytypeid += ite;
                            }
                            else
                            {
                                break;
                            }
                        }
                        Functions.IsEditEntity = false;
                        await Application.Current.MainPage.Navigation.PushAsync(new CreateEntityPage(System.Convert.ToInt32(entitytypeid), 0, "Create Entity", null));
                    }


                    #region View Case/Quest/Entity Page From Notes URL
                    if (appname == "ViewCases")
                    {
                        if (System.Convert.ToInt32(link_Nav_id) != 0)
                        {
                            Case_List Cl_obj = new Case_List();
                            try
                            {
                                var info = DBHelper.GetAppTypeInfoListContains_scrname("CASES", System.Convert.ToInt32(link_Nav_id), "E2_GetCaseList", App.DBPath);
                                info.Wait();
                                //GetCaseTypesResponse.BasicCase
                                if (info.Result != null)
                                {
                                    Cl_obj = new Case_List
                                    {
                                        CaseTypeId = System.Convert.ToInt32(info.Result.TYPE_ID),
                                        CaseID = System.Convert.ToInt32(info.Result.ID),
                                        CaseTypeName = info.Result.TYPE_NAME
                                    };
                                }
                                else
                                    Cl_obj = null;

                                if (CrossConnectivity.Current.IsConnected && Cl_obj == null)
                                {
                                    await System.Threading.Tasks.Task.Run(() =>
                                     {
                                         var result = CasesAPIMethods.GetCaseInfo(Functions.UserName, System.Convert.ToInt32(link_Nav_id));
                                         var cInfo = result.GetValue("ResponseContent").ToString();

                                         if (!string.IsNullOrEmpty(cInfo) && cInfo.ToString() != "[]")
                                         {
                                             Cl_obj = new Case_List();
                                             Cl_obj = JsonConvert.DeserializeObject<Case_List>(cInfo);
                                         }
                                         else
                                             Cl_obj = null;
                                     });
                                }

                                if (Cl_obj != null)
                                {
                                    await Application.Current.MainPage.Navigation.PushAsync(new ViewCasePage(System.Convert.ToString(Cl_obj.CaseID), System.Convert.ToString(Cl_obj.CaseTypeId), Cl_obj.CaseTypeName, ""));
                                }
                                else
                                {
                                    await Application.Current.MainPage.DisplayAlert("", "No Record Found. Please check Internet Connectivity.", "Ok");
                                }
                            }
                            catch (Exception e)
                            {
                            }
                        }
                    }
                    else if (appname == "ViewQuest")
                    {

                        if (System.Convert.ToInt32(link_Nav_id) != 0)
                        {
                            ItemInstanceTranByIDViewForm Quest_obj = new ItemInstanceTranByIDViewForm();
                            var info = DBHelper.GetAppTypeInfoListContains_scrname("QUEST", System.Convert.ToInt32(link_Nav_id), "H15_GetItemQuestion", App.DBPath);
                            info.Wait();

                            if (info.Result != null)
                            {
                                Quest_obj = new ItemInstanceTranByIDViewForm
                                {
                                    intAREA_ID = System.Convert.ToInt32(info.Result.CategoryId),
                                    intItemID = System.Convert.ToInt32(info.Result.TYPE_ID),
                                    intItemInstanceTranID = System.Convert.ToInt32(info.Result.ID),
                                    blnIsEdit = true
                                };
                            }
                            else
                                Quest_obj = null;


                            if (CrossConnectivity.Current.IsConnected && Quest_obj == null)
                            {
                                await System.Threading.Tasks.Task.Run(() =>
                                {
                                    var result = QuestAPIMethods.GetSecurityData(System.Convert.ToString(link_Nav_id), Functions.UserName);
                                    var QInfo = result.GetValue("ResponseContent").ToString();

                                    if (!string.IsNullOrEmpty(QInfo) && QInfo.ToString() != "[]")
                                    {
                                        Quest_obj = new ItemInstanceTranByIDViewForm();
                                        var lsQust = JsonConvert.DeserializeObject<List<ItemInstanceTranByIDViewForm>>(QInfo);

                                        if (lsQust.Count > 0)
                                        {
                                            Quest_obj = lsQust[0];
                                        }
                                    }
                                    else
                                        Quest_obj = null;
                                });
                            }

                            if (Quest_obj != null)
                            {
                                await Application.Current.MainPage.Navigation.PushAsync(new QuestItemDetailPage(System.Convert.ToString(Quest_obj.intItemID), System.Convert.ToString(Quest_obj.intItemInstanceTranID), "", "", "", System.Convert.ToBoolean(Quest_obj.blnIsEdit), System.Convert.ToString(Quest_obj.intAREA_ID)));
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("", "No Record Found. Please check Internet Connectivity.", "Ok");
                            }
                        }
                    }
                    else if (appname == "ViewEntities")
                    {
                        if (System.Convert.ToInt32(link_Nav_id) != 0)
                        {
                            EntityListMBView mbView = new EntityListMBView();

                            var info = DBHelper.GetAppTypeInfoListContains_scrname("ENTITY", System.Convert.ToInt32(link_Nav_id), "G8_EntityitemView", App.DBPath);
                            info.Wait();
                            if (info.Result != null)
                            {
                                mbView = new EntityListMBView
                                {
                                    EntityDetails = new EntityClass
                                    {
                                        EntityID = System.Convert.ToInt32(link_Nav_id),
                                        EntityTypeID = System.Convert.ToInt32(info.Result.TYPE_ID)
                                    },
                                    Title = info.Result.TYPE_NAME
                                };
                            }
                            else
                                mbView = null;

                            if (CrossConnectivity.Current.IsConnected && mbView == null)
                            {
                                await System.Threading.Tasks.Task.Run(() =>
                                {
                                    var result = EntityAPIMethods.GetEntityBasicDetails(System.Convert.ToInt32(link_Nav_id), Functions.UserName);
                                    var eInfo = result.GetValue("ResponseContent").ToString();
                                    if (!string.IsNullOrEmpty(eInfo) && eInfo.ToString() != "[]")
                                    {
                                        var tp = JsonConvert.DeserializeObject<EntityBasicDetails>(eInfo);
                                        mbView = new EntityListMBView
                                        {
                                            EntityDetails = new EntityClass
                                            {
                                                EntityID = System.Convert.ToInt32(link_Nav_id),
                                                EntityTypeID = System.Convert.ToInt32(tp.ENTITY_TYPE_ID)
                                            },
                                            Title = tp.ENTITY_TYPE_NAME
                                        };
                                    }
                                });
                            }

                            if (mbView != null)
                                await Application.Current.MainPage.Navigation.PushAsync(new Entity_View(mbView));
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("", "No Record Found. Please check Internet Connectivity.", "Ok");
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    Device.OpenUri(new Uri(url));
                }
            }
            catch (Exception ty)
            {
            }
        });

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    } 
}
