using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http.Headers;
using StemmonsMobile.Views.People_Screen;
using StemmonsMobile.Views.CreateQuestForm;
using StemmonsMobile.Views.Cases_Hopper_Center;
using StemmonsMobile.Views.View_Case_Origination_Center;
using DataServiceBus.OnlineHelper.DataTypes;
using DataServiceBus;
using StemmonsMobile.DataTypes.DataType.Quest;
using DataServiceBus.OfflineHelper.DataTypes;
using DataServiceBus.OfflineHelper.DataTypes.Common;

namespace StemmonsMobile.Commonfiles
{
    class APICalls
    {
        public static string Grant_type = "password";
        public static string Username = "api_admin";
        public static string Password = "Boxer@123";

        #region Done By Vishal Prajapati

        public static JObject AllpostAPIcall(List<KeyValuePair<string, string>> APIDetails, List<KeyValuePair<string, string>> BodyValue)
        {
            JObject obj = null;
            try
            {
                string url = APIDetails[0].Value;
                string Api_Type = APIDetails[1].Value;
                string _ContentType = "application/json";

                HttpClient httpClient = new HttpClient();

                HttpRequestMessage httpContent;
                httpContent = new HttpRequestMessage(HttpMethod.Post, url);
                httpContent.Headers.ExpectContinue = false;
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Functions.access_token);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_ContentType));

                Task<HttpResponseMessage> response;

                if (Api_Type == "POST")
                {
                    httpContent.Content = new FormUrlEncodedContent(BodyValue);
                    response = httpClient.SendAsync(httpContent);
                }
                else
                {
                    response = httpClient.GetAsync(url);
                }

                var result = response.Result.Content.ReadAsStringAsync();
                obj = JObject.Parse(result.Result);
            }
            catch (Exception e)
            {
            }
            return obj;

        }
     

        //        #region API Body Details
        //        var Body_value = new List<KeyValuePair<string, string>>
        //    {
        //        new KeyValuePair<string, string>("grant_type", Grant_type),
        //        new KeyValuePair<string, string>("username", Username),
        //        new KeyValuePair<string, string>("password", Password)
        //    };
        //        #endregion

        //        var val = AllpostAPIcall(API_value, Body_value);
        //        if (val != null)
        //            Functions.access_token = val.GetValue("access_token").ToString();
        //        Debug.WriteLine("URL ==> " + Functions.Baseurl + Constants.Get_Token + Environment.NewLine + "access_token ==> " + Functions.access_token);
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        #region Entity APIs

        //public static JObject GetEntityRelateApplication(string ETypeId, string EId)
        //{
        //    #region API Details
        //    var API_value = new List<KeyValuePair<string, string>>
        //    {
        //        new KeyValuePair<string, string>("API_Name", Functions.Baseurl + Constants.Get_EntityRelateApp),
        //        new KeyValuePair<string, string>("API_Type", "POST")
        //    };
        //    #endregion

        //    #region API Body Details
        //    var Body_value = new List<KeyValuePair<string, string>>
        //    {
        //        new KeyValuePair<string, string>("user", Functions._UserName),
        //         new KeyValuePair<string, string>("EntityTypeId", ETypeId),
        //          new KeyValuePair<string, string>("EntityId", EId),
        //    };
        //    #endregion

        //    var val = AllpostAPIcall(API_value, Body_value);
        //    if (val != null)
        //    {
        //        return val;
        //    }
        //    else
        //        return null;
        //}

        //public static JObject GetEntityRelateApplicationType(string ETypeId, string EId, string AppName)
        //{
        //    #region API Details
        //    var API_value = new List<KeyValuePair<string, string>>
        //    {
        //        new KeyValuePair<string, string>("API_Name", Functions.Baseurl + Constants.Get_EntityRelateAppType),
        //        new KeyValuePair<string, string>("API_Type", "POST")
        //    };
        //    #endregion

        //    #region API Body Details
        //    var Body_value = new List<KeyValuePair<string, string>>
        //    {
        //        new KeyValuePair<string, string>("user", Functions._UserName),
        //         new KeyValuePair<string, string>("EntityTypeId", ETypeId),
        //          new KeyValuePair<string, string>("EntityId", EId),
        //           new KeyValuePair<string, string>("Application", AppName),
        //    };
        //    #endregion

        //    var val = AllpostAPIcall(API_value, Body_value);
        //    if (val != null)
        //    {
        //        return val;
        //    }
        //    else
        //        return null;
        //}

        //public static JObject GetEntityRelateApplicationData(object Body_value)
        //{
        //    try
        //    {
        //        return Apica(Body_value, Constants.Get_EntityRelationData);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        //public static JObject Apica(object Body_value, string ApiName)
        //{
        //    try
        //    {
        //        #region API Details
        //        var API_value = new List<KeyValuePair<string, string>>
        //    {
        //        new KeyValuePair<string, string>("API_Name",Functions.Baseurl  + ApiName)
        //    };
        //        #endregion

        //        var Result = MobileAPIMethods.CallAPIGetPostList(API_value, Body_value);
        //        if (Result != null)
        //        {
        //            return JObject.Parse(Result.ToString());
        //        }
        //        else
        //            return null;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        #endregion

        #endregion


        #region Done Bhavin Joshi

        //Quest Api
        #region Quest

        //public static List<QuestQuestionData> GetItemQuestionMetadata(string itemtransID)
        //{
        //    #region API Details
        //    var API_value = new List<KeyValuePair<string, string>>
        //    {
        //        new KeyValuePair<string, string>("API_Name", Functions.Baseurl + Constants.GetItemQuestionMetadata),
        //        new KeyValuePair<string, string>("API_Type", "POST")
        //    };

        //    #endregion

        //    #region API Body Details
        //    var Body_value = new List<KeyValuePair<string, string>>
        //    {
        //        new KeyValuePair<string, string>("intItemInstanceTranID",itemtransID),
        //    };
        //    #endregion

        //    var val = AllpostAPIcall(API_value, Body_value);

        //    var temp = val.GetValue("ResponseContent");

        //    List<QuestQuestionData> lst = new List<QuestQuestionData>();
        //    foreach (var item in temp)
        //    {
        //        if (item.ToString() == "[" || item.ToString() == "]" || item.ToString() == "{" || item.ToString() == "}")
        //        {
        //            continue;
        //        }
        //        QuestQuestionData questdata;
        //        questdata = Newtonsoft.Json.JsonConvert.DeserializeObject<QuestQuestionData>(item.ToString());
        //        lst.Add(questdata);
        //    }
        //    return lst;
        //}

        //public static List<QuestQuestionViewFormData> GetItemQuestionMetadata1(string itemtransID)
        //{
        //    #region API Details
        //    var API_value = new List<KeyValuePair<string, string>>
        //    {
        //        new KeyValuePair<string, string>("API_Name", Functions.Baseurl + Constants.GetItemQuestionMetadata),
        //        new KeyValuePair<string, string>("API_Type", "POST")
        //    };

        //    #endregion

        //    #region API Body Details
        //    var Body_value = new List<KeyValuePair<string, string>>
        //    {
        //        new KeyValuePair<string, string>("intItemInstanceTranID",itemtransID),
        //    };
        //    #endregion

        //    var val = AllpostAPIcall(API_value, Body_value);

        //    var temp = val.GetValue("ResponseContent");

        //    List<QuestQuestionViewFormData> lst = new List<QuestQuestionViewFormData>();
        //    foreach (var item in temp)
        //    {
        //        if (item.ToString() == "[" || item.ToString() == "]" || item.ToString() == "{" || item.ToString() == "}")
        //        {
        //            continue;

        //        }
        //        QuestQuestionViewFormData questdata;
        //        questdata = Newtonsoft.Json.JsonConvert.DeserializeObject<QuestQuestionViewFormData>(item.ToString());
        //        lst.Add(questdata);
        //    }
        //    return lst;
        //}

        //public static JObject GetCountForQuestItem(string username, string itemtID)
        //{
        //    #region API Details
        //    var API_value = new List<KeyValuePair<string, string>>
        //    {
        //        new KeyValuePair<string, string>("API_Name", Functions.Baseurl + Constants.GetCountForQuestItem),
        //        new KeyValuePair<string, string>("API_Type", "POST")
        //    };

        //    #endregion

        //    #region API Body Details
        //    var Body_value = new List<KeyValuePair<string, string>>
        //    {
        //        new KeyValuePair<string, string>("ItemId",itemtID),
        //        new KeyValuePair<string, string>("User",username)
        //    };
        //    #endregion

        //    var val = AllpostAPIcall(API_value, Body_value);
        //    // var val = AllpostAPIcall(API_value, Body_value);
        //    if (val != null)
        //    {
        //        return val;
        //    }
        //    else
        //        return null;

        //}

        public static JObject GetFilesByQuestionID(string itemQuestionMetadataID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", DataServiceBus.OnlineHelper.DataTypes.Constants.Baseurl + Constants.GetFilesByQuestionID),
            };

            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("itemQuestionMetaDataID",itemQuestionMetadataID)
            };
            #endregion

            var val = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (val != null)
            {
                return val;
            }
            else
                return null;

        }

        #region Get Files By Question ID
        public async static Task<List<GetFilesByQuestionIDResponse.GetFilesByQuestionIDModel>> GetFilesByQuestionID(bool _IsOnline, string _intItemID, int _InstanceUserAssocId, string _DBPath)
        {
            List<GetFilesByQuestionIDResponse.GetFilesByQuestionIDModel> ItemList = new List<GetFilesByQuestionIDResponse.GetFilesByQuestionIDModel>();
            List<AppTypeInfoList> lstResult = new List<AppTypeInfoList>();
            int id = CommonConstants.GetResultBySytemcode(ConstantsSync.QuestInstance, "H9_GetFilesByQuestionID", _DBPath);
            try
            {
                if (_IsOnline)
                {
                    var result = APICalls.GetFilesByQuestionID(_intItemID);
                    var temp = result.GetValue("ResponseContent");
                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        ItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetFilesByQuestionIDResponse.GetFilesByQuestionIDModel>>(temp.ToString());
                        if (ItemList.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(ItemList), ConstantsSync.QuestInstance, "H9_GetFilesByQuestionID", _InstanceUserAssocId, _DBPath, id, _intItemID,"M");
                        }
                    }
                }
                else
                {
                    ItemList = CommonConstants.ReturnListResult<GetFilesByQuestionIDResponse.GetFilesByQuestionIDModel>(ConstantsSync.QuestInstance, "H9_GetFilesByQuestionID", _DBPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ItemList;
        }
        #endregion

        #endregion        

        #endregion

    }
}

