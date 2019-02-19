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
using System.Text;
using System.Net;
using HtmlAgilityPack;

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
            HtmlToText convert = new HtmlToText();
            var sections = new List<StringSection>();
            try
            {
                //rawText = rawText.Replace("<p>", Environment.NewLine);
                rawText = rawText.Replace("&nbsp;", " ");
                rawText = rawText.Replace("&amp;", "&");
                //rawText = rawText.Replace("<br>", Environment.NewLine);
                //rawText = rawText.Replace("</br>", Environment.NewLine);
                //rawText = rawText.Replace("<br/>", Environment.NewLine);
                //rawText = rawText.Replace("<br />", Environment.NewLine);
                //// rawText = rawText.Replace("<br ", Environment.NewLine);

                //rawText = rawText.Replace("<p>", Environment.NewLine);
                //rawText = rawText.Replace("</p>", Environment.NewLine);
                //// rawText = rawText.Replace("<p ", Environment.NewLine);


                //StringBuilder sbHTML = new StringBuilder(rawText);
                //string[] OldWords = { "&nbsp;", "&amp;", "&quot;", "&lt;", "&gt;", "&reg;", "&copy;", "&bull;", "&trade;", "&#39;", "&rsquo;", "&lsquo;", "&sbquo;", "&ldquo;", "&rdquo;", "&bdquo;", "&frasl;", "&#8217;" };
                //string[] NewWords = { " ", "&", "\"", "<", ">", "Â®", "Â©", "•", "™", "'", "’", "‘", "‚", "“", "”", "„", "⁄", "’" };
                //for (int i = 0; i < OldWords.Length; i++)
                //{
                //    sbHTML = sbHTML.Replace(OldWords[i], NewWords[i]);
                //}
                //rawText = sbHTML.ToString();

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(rawText);

                var htmlBody = htmlDoc.DocumentNode.DescendantsAndSelf("body");

                foreach (var item in htmlBody)
                {
                    if (item.Name == "body")
                    {
                        rawText = item.InnerHtml;
                    }
                }

                List<StringSection> list = new List<StringSection>();
                const string spanPattern = @"(<a.*?>.*?</a>)";
                const string spanPattern1 = @"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?";

                MatchCollection collection1 = Regex.Matches(rawText, spanPattern1, RegexOptions.Singleline);// For URL

                MatchCollection collection = Regex.Matches(rawText, spanPattern, RegexOptions.Singleline); // For HyperLink


                #region MyRegion
                //// 1.
                //// Find all matches in file.
                //MatchCollection m1 = Regex.Matches(rawText, spanPattern, RegexOptions.Singleline);

                //// 2.
                //// Loop over each match.
                //foreach (Match m in m1)
                //{
                //    string value = m.Groups[1].Value;
                //    StringSection i = new StringSection();

                //    // 3.
                //    // Get href attribute.
                //    Match m2 = Regex.Match(value, @"href=\""(.*?)\""",
                //    RegexOptions.Singleline);
                //    if (m2.Success)
                //    {
                //        i.Link = m2.Groups[1].Value;
                //    }

                //    // 4.
                //    // Remove inner tags from text.
                //    string ta = Regex.Replace(value, @"\s*<.*?>\s*", "",
                //    RegexOptions.Singleline);
                //    i.Text = ta;

                //    list.Add(i);

                //} 
                #endregion

                var lastIndex = 0;
                bool Isadded = false;
                foreach (Match item in collection)
                {
                    Isadded = true;
                    var foundText = item.Value;
                    sections.Add(new StringSection() { Text = convert.Convert(rawText.Substring(lastIndex, item.Index - lastIndex)) });
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

                if (!Isadded)
                {
                    lastIndex = 0;
                    foreach (Match item in collection1)
                    {
                        var foundText = item.Value;
                        sections.Add(new StringSection() { Text = convert.Convert(rawText.Substring(lastIndex, item.Index - lastIndex)) });
                        lastIndex = item.Index + item.Length;

                        // Get HTML href 
                        var html = new StringSection()
                        {
                            Link = Regex.Match(item.Value, spanPattern1).Value,
                            Text = Regex.Replace(item.Value, "<.*?>", string.Empty)
                        };

                        sections.Add(html);
                    }
                }

                var te = convert.Convert(rawText.Substring(lastIndex));
                var ysa = ConvertUrlsToLinks(rawText.Substring(lastIndex));
                sections.Add(new StringSection() { Text = te });

                return sections;

            }
            catch (Exception)
            {
            }
            var ts = convert.Convert(rawText.Substring(0));
            var ys = ConvertUrlsToLinks(ts);
            //var temp = WebUtility.HtmlDecode(rawText);
            sections.Add(new StringSection() { Text = ts });

            return sections;
        }

        public class StringSection
        {
            public string Text { get; set; }
            public string Link { get; set; }
        }

        private static string ConvertUrlsToLinks(string arg)
        {
            //Replaces web and email addresses in text with hyperlinks
            Regex urlregex = new Regex(@"(^|[\n ])(?<url>(www|ftp)\.[^ ,""\s<]*)", RegexOptions.IgnoreCase | RegexOptions.None);
            arg = urlregex.Replace(arg, " <a href=\"http://${url}\" target=\"_blank\">${url}</a>");
            Regex httpurlregex = new Regex(@"(^|[\n ])(?<url>(http://www\.|http://|https://)[^ ,""\s<]*)", RegexOptions.IgnoreCase | RegexOptions.None);
            arg = httpurlregex.Replace(arg, " <a href=\"${url}\" target=\"_blank\">${url}</a>");
            Regex emailregex = new Regex(@"(?<url>[a-zA-Z_0-9.-]+\@[a-zA-Z_0-9.-]+\.\w+\s)", RegexOptions.IgnoreCase | RegexOptions.None);
            arg = emailregex.Replace(arg, " <a href=\"mailto:${url}\">${url}</a> ");
            return arg;
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

                var CheckList = JsonConvert.DeserializeObject<List<MobileBranding>>(tempU.ASSOC_FIELD_INFO);
                List<string> URLlist = new List<string>();
                foreach (var item in CheckList)
                {
                    MatchCollection collection = Regex.Matches(item.VALUE.ToLower(), @"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?", RegexOptions.Singleline);

                    foreach (Match itm in collection)
                    {
                        URLlist.Add(itm.Value);
                    }
                }

                bool isopenWeb = true;
                foreach (var tURL in URLlist)
                {
                    if (url.Replace("https://", "").Replace("http://", "").Contains(tURL.Replace("https://", "").Replace("http://", "").ToLower()))
                    {
                        isopenWeb = false;
                        break;
                    }
                    else
                    {
                        isopenWeb = true;
                    }
                }

                //This condition is for Boxer Property only
                if (url.Contains("http://mobileservices.boxerproperty.com/mobileservice"))
                    isopenWeb = true;

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
                    else if (url.Contains("/downloadfile.aspx?") || url.Contains("/download.aspx?") || url.Contains("entitiesdownload.aspx?"))
                    {
                        if (url.Contains("?casefileid=")) //Case attachment view
                        {
                            await Application.Current.MainPage.Navigation.PushAsync(new StemmonsMobile.Views.ViewAttachment
                                                    (url, "Cases"));
                        }
                        else if (url.Contains("entityid=")) //Entities attachment view
                        {
                            await Application.Current.MainPage.Navigation.PushAsync(new StemmonsMobile.Views.ViewAttachment
                                                    (url, "Entities"));
                        }
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



    class HtmlToText
    {
        // Static data tables
        protected static Dictionary<string, string> _tags;
        protected static HashSet<string> _ignoreTags;

        // Instance variables
        protected TextBuilder _text;
        protected string _html;
        protected int _pos;

        // Static constructor (one time only)
        static HtmlToText()
        {
            _tags = new Dictionary<string, string>();
            _tags.Add("address", "\n");
            _tags.Add("blockquote", "\n");
            _tags.Add("div", "\n");
            _tags.Add("dl", "\n");
            _tags.Add("fieldset", "\n");
            _tags.Add("form", "\n");
            _tags.Add("h1", "\n");
            _tags.Add("/h1", "\n");
            _tags.Add("h2", "\n");
            _tags.Add("/h2", "\n");
            _tags.Add("h3", "\n");
            _tags.Add("/h3", "\n");
            _tags.Add("h4", "\n");
            _tags.Add("/h4", "\n");
            _tags.Add("h5", "\n");
            _tags.Add("/h5", "\n");
            _tags.Add("h6", "\n");
            _tags.Add("/h6", "\n");
            _tags.Add("p", "\n");
            _tags.Add("/p", "\n");
            _tags.Add("table", "\n");
            _tags.Add("/table", "\n");
            _tags.Add("ul", "\n");
            _tags.Add("/ul", "\n");
            _tags.Add("ol", "\n");
            _tags.Add("/ol", "\n");
            _tags.Add("/li", "\n");
            _tags.Add("br", "\n");
            _tags.Add("/td", "\t");
            _tags.Add("/tr", "\n");
            _tags.Add("/pre", "\n");

            _ignoreTags = new HashSet<string>();
            _ignoreTags.Add("script");
            _ignoreTags.Add("noscript");
            _ignoreTags.Add("style");
            _ignoreTags.Add("object");
        }

        /// <summary>
        /// Converts the given HTML to plain text and returns the result.
        /// </summary>
        /// <param name="html">HTML to be converted</param>
        /// <returns>Resulting plain text</returns>
        public string Convert(string html)
        {
            // Initialize state variables
            _text = new TextBuilder();
            _html = html;
            _pos = 0;

            // Process input
            while (!EndOfText)
            {
                if (Peek() == '<')
                {
                    // HTML tag
                    bool selfClosing;
                    string tag = ParseTag(out selfClosing);

                    // Handle special tag cases
                    if (tag == "body")
                    {
                        // Discard content before <body>
                        _text.Clear();
                    }
                    else if (tag == "/body")
                    {
                        // Discard content after </body>
                        _pos = _html.Length;
                    }
                    else if (tag == "pre")
                    {
                        // Enter preformatted mode
                        _text.Preformatted = true;
                        EatWhitespaceToNextLine();
                    }
                    else if (tag == "/pre")
                    {
                        // Exit preformatted mode
                        _text.Preformatted = false;
                    }

                    string value;
                    if (_tags.TryGetValue(tag, out value))
                        _text.Write(value);

                    if (_ignoreTags.Contains(tag))
                        EatInnerContent(tag);
                }
                else if (Char.IsWhiteSpace(Peek()))
                {
                    // Whitespace (treat all as space)
                    _text.Write(_text.Preformatted ? Peek() : ' ');
                    MoveAhead();
                }
                else
                {
                    // Other text
                    _text.Write(Peek());
                    MoveAhead();
                }
            }
            // Return result
            return WebUtility.HtmlDecode(_text.ToString());
        }

        // Eats all characters that are part of the current tag
        // and returns information about that tag
        protected string ParseTag(out bool selfClosing)
        {
            string tag = String.Empty;
            selfClosing = false;

            if (Peek() == '<')
            {
                MoveAhead();

                // // Parse tag name
                //atWhitespace();
                int start = _pos;
                if (Peek() == '/')
                    MoveAhead();
                while (!EndOfText && !Char.IsWhiteSpace(Peek()) &&
                    Peek() != '/' && Peek() != '>')
                    MoveAhead();
                tag = _html.Substring(start, _pos - start).ToLower();

                // Parse rest of tag
                while (!EndOfText && Peek() != '>')
                {
                    if (Peek() == '"' || Peek() == '\'')
                        EatQuotedValue();
                    else
                    {
                        if (Peek() == '/')
                            selfClosing = true;
                        MoveAhead();
                    }
                }
                MoveAhead();
            }
            return tag;
        }

        // Consumes inner content from the current tag
        protected void EatInnerContent(string tag)
        {
            string endTag = "/" + tag;

            while (!EndOfText)
            {
                if (Peek() == '<')
                {
                    // Consume a tag
                    bool selfClosing;
                    if (ParseTag(out selfClosing) == endTag)
                        return;
                    // Use recursion to consume nested tags
                    if (!selfClosing && !tag.StartsWith("/"))
                        EatInnerContent(tag);
                }
                else
                    MoveAhead();
            }
        }

        // Returns true if the current position is at the end of
        // the string
        protected bool EndOfText
        {
            get { return (_pos >= _html.Length); }
        }

        // Safely returns the character at the current position
        protected char Peek()
        {
            return (_pos < _html.Length) ? _html[_pos] : (char)0;
        }

        // Safely advances to current position to the next character
        protected void MoveAhead()
        {
            _pos = Math.Min(_pos + 1, _html.Length);
        }

        // Moves the current position to the next non-whitespace
        // character.
        protected void EatWhitespace()
        {
            while (Char.IsWhiteSpace(Peek()))
                MoveAhead();
        }

        // Moves the current position to the next non-whitespace
        // character or the start of the next line, whichever
        // comes first
        protected void EatWhitespaceToNextLine()
        {
            while (Char.IsWhiteSpace(Peek()))
            {
                char c = Peek();
                MoveAhead();
                if (c == '\n')
                    break;
            }
        }

        // Moves the current position past a quoted value
        protected void EatQuotedValue()
        {
            char c = Peek();
            if (c == '"' || c == '\'')
            {
                // Opening quote
                MoveAhead();
                // Find end of value
                int start = _pos;
                _pos = _html.IndexOfAny(new char[] { c, '\r', '\n' }, _pos);
                if (_pos < 0)
                    _pos = _html.Length;
                else
                    MoveAhead();    // Closing quote
            }
        }

        /// <summary>
        /// A StringBuilder class that helps eliminate excess whitespace.
        /// </summary>
        protected class TextBuilder
        {
            private StringBuilder _text;
            private StringBuilder _currLine;
            private int _emptyLines;
            private bool _preformatted;

            // Construction
            public TextBuilder()
            {
                _text = new StringBuilder();
                _currLine = new StringBuilder();
                _emptyLines = 0;
                _preformatted = false;
            }

            /// <summary>
            /// Normally, extra whitespace characters are discarded.
            /// If this property is set to true, they are passed
            /// through unchanged.
            /// </summary>
            public bool Preformatted
            {
                get
                {
                    return _preformatted;
                }
                set
                {
                    if (value)
                    {
                        // Clear line buffer if changing to
                        // preformatted mode
                        if (_currLine.Length > 0)
                            FlushCurrLine();
                        _emptyLines = 0;
                    }
                    _preformatted = value;
                }
            }

            /// <summary>
            /// Clears all current text.
            /// </summary>
            public void Clear()
            {
                _text.Length = 0;
                _currLine.Length = 0;
                _emptyLines = 0;
            }

            /// <summary>
            /// Writes the given string to the output buffer.
            /// </summary>
            /// <param name="s"></param>
            public void Write(string s)
            {
                foreach (char c in s)
                    Write(c);
            }

            /// <summary>
            /// Writes the given character to the output buffer.
            /// </summary>
            /// <param name="c">Character to write</param>
            public void Write(char c)
            {
                if (_preformatted)
                {
                    // Write preformatted character
                    _text.Append(c);
                }
                else
                {
                    if (c == '\r')
                    {
                        // Ignore carriage returns. We'll process
                        // '\n' if it comes next
                    }
                    else if (c == '\n')
                    {
                        // Flush current line
                        FlushCurrLine();
                    }
                    else if (Char.IsWhiteSpace(c))
                    {
                        // Write single space character
                        int len = _currLine.Length;
                        if (len == 0 || !Char.IsWhiteSpace(_currLine[len - 1]))
                            _currLine.Append(' ');
                    }
                    else
                    {
                        // Add character to current line
                        _currLine.Append(c);
                    }
                }
            }

            // Appends the current line to output buffer
            protected void FlushCurrLine()
            {
                // Get current line
                string line = _currLine.ToString().Trim();

                // Determine if line contains non-space characters
                string tmp = line.Replace("&nbsp;", " ");
                if (tmp.Length == 0)
                {
                    // An empty line
                    _emptyLines++;
                    if (_emptyLines < 2 && _text.Length > 0)
                        _text.AppendLine(line);
                }
                else
                {
                    // A non-empty line
                    _emptyLines = 0;
                    _text.AppendLine(line);
                }

                // Reset current line
                _currLine.Length = 0;
            }

            /// <summary>
            /// Returns the current output as a string.
            /// </summary>
            public override string ToString()
            {
                if (_currLine.Length > 0)
                    FlushCurrLine();
                return _text.ToString();
            }
        }
    }
}
