using Newtonsoft.Json.Linq;
using StemmonsMobile.DataTypes.DataType.Cases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;
using static StemmonsMobile.DataTypes.DataType.Cases.GetCaseTypesRequest;

namespace DataServiceBus.OnlineHelper.DataTypes
{
    public class CasesAPIMethods
    {
        #region Get User Info  
        public static JObject GetUserInfo(string _userName)
        {
            MobileAPIMethods Mapi = new MobileAPIMethods();
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl  + Constants.GetUserInfo)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("userName", _userName),
            };
            #endregion
            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Add Case Notes        
        public static JObject AddCaseNotes(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.AddCaseNotes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get Hopper With Owner By Username
        public static JObject GetHopperWithOwnerByUsername(string userName)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetHopperWithOwnerByUsername)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("userName", userName)


            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Case Note Types
        public static JObject GetCaseNoteTypes()
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",  Constants.Baseurl + Constants.GetCaseNoteTypes)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, null, "GET");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Index
        public static JObject Index()
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",  Constants.Baseurl + Constants.Index)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, null, "GET");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Case Notes
        public static JObject GetCaseNotes(string CaseID, string CreatedDate)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetCaseNotes)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("CaseID", CaseID),
                 new KeyValuePair<string, string>("CreatedDate", CreatedDate)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Close Case
        public static JObject CloseCase(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.CloseCase);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ReOpen Case
        public static JObject ReOpenCase(string caseID, string user)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.ReOpenCase)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("caseId", caseID),
                new KeyValuePair<string, string>("user", user)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Case List By Hopper
        public static JObject GetCaseListByHopper(string userAccount, string showAll)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetCaseListByHopper)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("userAccount", userAccount),
                new KeyValuePair<string, string>("showAll", showAll)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Hopper Center By User    
        public static JObject GetHopperCenterByUser(string userAccount, string showAll)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetHopperCenterByUser)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("userAccount", userAccount),
                new KeyValuePair<string, string>("showAll", showAll)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Case Types
        public static JObject GetCaseTypes()
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.getCaseTypes)
            };
            #endregion            

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, null, "GET");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Origination Center For User
        public static JObject GetOriginationCenterForUser(string user, string showAll)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetOriginationCenterForUser)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("user", user),
                new KeyValuePair<string, string>("showAll", showAll)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Case Types User CaseTypeID
        public static JObject GetCaseTypesUserCaseTypeID(string userName, string caseTypeId)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.getCaseTypesUserCaseTypeID)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("userName", userName),
                new KeyValuePair<string, string>("caseTypeId", caseTypeId)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Case List
        public static JObject GetCaseList(string user, string CaseTypeID = null, string CaseOwnerSAM = null, string AssignedToSAM = null, string ClosedBySAM = null, string CreatedBySAM = null,
                                                    string PropertyID = null, string TenantCode = null, string TenantID = null, string showOpenClosedCasesType = "O", string showPastDueDate = "N", string SearchQuery = "", string Pagenumber = "0", string Pageindex = "0")
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetCaseList)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("userName", user),
                new KeyValuePair<string, string>("caseTypeId", CaseTypeID),
                new KeyValuePair<string, string>("caseOwnerSAM", CaseOwnerSAM),
                new KeyValuePair<string, string>("caseAssgnSAM", AssignedToSAM),
                new KeyValuePair<string, string>("caseCloseBySAM", ClosedBySAM),
                new KeyValuePair<string, string>("caseCreateBySAM", CreatedBySAM),
                new KeyValuePair<string, string>("propertyId", PropertyID),
                new KeyValuePair<string, string>("tenant_Code", TenantCode),
                new KeyValuePair<string, string>("tenant_Id", TenantID),
                new KeyValuePair<string, string>("showOpenClosedCasesType", showOpenClosedCasesType),
                new KeyValuePair<string, string>("showPastDueCases", showPastDueDate),
                new KeyValuePair<string, string>("searchQuery", SearchQuery),
                new KeyValuePair<string, string>("Pagenumber", Pagenumber),
                new KeyValuePair<string, string>("Pageindex", Pageindex),
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Case List
        public static JObject GetCaseListSync(string user, string CaseTypeID = null, string CaseOwnerSAM = null, string AssignedToSAM = null, string ClosedBySAM = null, string CreatedBySAM = null, List<KeyValuePair<string, string>> keyValuePairs = null, string PropertyID = null, string TenantCode = null, string TenantID = "0", char showOpenClosedCasesType = 'O', char showPastDueDate = 'N', string SearchQuery = "", string screenName = "", int? _pageindex = 0, int? _pagenumber = 0)
        {
            //#region API Details
            //var API_value = new List<KeyValuePair<string, string>>
            //{
            //    new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetCaseListSync)
            //};
            //#endregion

            #region API Body Details

            GetCaseListRequest req = new GetCaseListRequest();
            req.userName = user;
            req.caseTypeId = CaseTypeID;
            req.caseOwnerSAM = CaseOwnerSAM;
            req.caseAssgnSAM = AssignedToSAM;
            req.caseCloseBySAM = ClosedBySAM;
            req.caseCreateBySAM = CreatedBySAM;
            req.propertyId = PropertyID;
            req.tenant_Code = TenantCode;
            req.Pageindex = _pageindex;
            req.Pagenumber = _pagenumber;
            try
            {
                req.tenant_Id = Convert.ToInt32(TenantID);
            }
            catch (Exception)
            {
                req.tenant_Id = 0;
            }
            req.showOpenClosedCasesType = showOpenClosedCasesType;
            req.showPastDueCases = showPastDueDate;
            req.keyValuePairs = keyValuePairs;
            req.searchQuery = SearchQuery;
            req.ScreenName = screenName;

            try
            {
                return Constants.ApiCommon(req, Constants.GetCaseListSync);
            }
            catch (Exception)
            {
                throw;
            }

            #endregion
        }
        #endregion

        #region Create Case        
        public static JObject CreateCase(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.CreateCase);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Assign Case
        public static JObject AssignCase(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.AssignCase);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Case Types To Created By User
        public static JObject GetCaseTypesToCreateByUser(string username)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetCaseTypesToCreateByUser)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", username )
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get System Configuration Values
        public static JObject GetSystemConfigurationValues()
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetSystemConfigurationValues)
            };
            #endregion


            var Result = MobileAPIMethods.CallAPIGetPost(API_value, null, "GET");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Unsubscribe From Alert
        public static JObject UnsubscribeFromAlert(string caseTypeId, string emailAddress, string username)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.UnsubscribeFromAlert)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("caseTypeId", caseTypeId),
                new KeyValuePair<string, string>("emailAddress", emailAddress),
                new KeyValuePair<string, string>("username", username )
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion        

        #region Subscribe To Alert
        public static JObject SubscribeToAlert(string caseId, string caseTypeId, string emailAddress, string username)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.SubscribeToAlert)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("caseId", caseId),
                new KeyValuePair<string, string>("caseTypeId", caseTypeId),
                new KeyValuePair<string, string>("emailAddress", emailAddress),
                new KeyValuePair<string, string>("username", username )
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get CaseType Name
        public static JObject GetCaseTypeName(string caseTypeId, string emailAddress, string username)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetCaseTypeName)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("caseTypeId", caseTypeId),
                new KeyValuePair<string, string>("emailAddress", emailAddress),
                new KeyValuePair<string, string>("username", username )
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get CaseType Name
        public static JObject GetTypesByCaseTypeID(string caseTypeId, string currentUser)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetTypesByCaseTypeID)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("currentUser", currentUser),
                new KeyValuePair<string, string>("caseTypeId", caseTypeId)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Type Values By AssocCaseType ExternalDS
        public static JObject GetTypeValuesByAssocCaseTypeExternalDS(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.GetTypeValuesByAssocCaseTypeExternalDS);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Get CaseType Name
        public static JObject GetExdCascadedValue(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.GetExdCascadedValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Type Values By AssocCaseType
        public static JObject GetTypeValuesByAssocCaseType(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.GetTypeValuesByAssocCaseType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Types By CaseTypeID Raw
        public static JObject GetTypesByCaseTypeIDRaw(string caseTypeID, string username)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetTypesByCaseTypeIDRaw)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("caseTypeID", caseTypeID),
                new KeyValuePair<string, string>("username", username ),
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Types By CaseTypeID Raw
        public static JObject GetCaseTypeDataByUser(string caseTypeID, string username)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetCaseTypeDataByUser)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("caseTypeID", caseTypeID),
                 new KeyValuePair<string, string>("username", username ),
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Type Values By ParentID
        public static JObject GetTypeValuesByParentID(string selectedParentId)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetTypeValuesByParentID)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("selectedParentId", selectedParentId),
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Upload File To Case
        public static JObject UploadFileToCase(int caseNumber, string fileDescription, DateTime dateTime, string fileName, string fileSize, byte[] fileBinary, string externalURI, char addToCaseNotes, string systemCode, char isActive, string currentUser)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.UploadFileToCase)
            };
            #endregion

            #region API Body Details
            UploadFileToCaseTypeModel uploadfile = new UploadFileToCaseTypeModel();
            uploadfile.caseNumber = caseNumber;
            uploadfile.fileDescription = fileDescription;
            uploadfile.dateTime = Convert.ToDateTime(dateTime);
            uploadfile.fileName = fileName;
            uploadfile.fileBinary = fileBinary;
            uploadfile.externalURI = externalURI;
            uploadfile.addToCaseNotes = addToCaseNotes;
            uploadfile.systemCode = systemCode;
            uploadfile.isActive = isActive;
            uploadfile.currentUser = currentUser;

            #endregion
            try
            {
                return Constants.ApiCommon(uploadfile, Constants.UploadFileToCase);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Upload File to Case usign Multipart
        //public static string UploadFileToCase(int caseNumber, string fileDescription, DateTime dateTime, string fileName, string fileSize, byte[] fileBinary, string externalURI, char addToCaseNotes, string systemCode, char isActive, string currentUser)
        //{

        //    #region MyRegion
        //    try
        //    {
        //        string fileID = "-1";
        //        using (HttpClient httpClient = new HttpClient())
        //        {
        //            string url = Constants.Baseurl + Constants.UploadFileToCase;

        //            url += "?caseNumber=" + caseNumber + "&fileDescription=NewFile&dateTime=" + DateTime.Now.Date + "&externalURI=" + externalURI + "&addToCaseNotes=Y&systemCode=null&isActive=Y&currentUser=" + currentUser;


        //            httpClient.Timeout = new TimeSpan(1, 10, 0);
        //            string accessToken = MobileAPIMethods.RequestToken(Constants.Baseurl + Constants.Get_Token);
        //            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);


        //            var values = new[]
        //                       {
        //                                    new KeyValuePair<string, string>("caseNumber", caseNumber.ToString()),
        //                                    new KeyValuePair<string, string>("fileDescription", ""),
        //                                    new KeyValuePair<string, string>("dateTime", DateTime.Now.ToString()),
        //                                    new KeyValuePair<string, string>("fileName", fileName),
        //                                    new KeyValuePair<string, string>("fileBinary", fileBinary.ToString()),
        //                                    new KeyValuePair<string, string>("externalURI", ""),
        //                                    new KeyValuePair<string, string>("addToCaseNotes", "Y"),
        //                                    new KeyValuePair<string, string>("systemCode", "Less"),
        //                                    new KeyValuePair<string, string>("isActive", "y"),
        //                                    new KeyValuePair<string, string>("currentUser", currentUser),

        //                                };

        //            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
        //            requestMessage.Headers.ExpectContinue = false;

        //            MultipartFormDataContent multiPartContent = new MultipartFormDataContent("------------------------99914737809831466499882746641441");
        //            ByteArrayContent byteArrayContent = new ByteArrayContent(fileBinary);
        //            byteArrayContent.Headers.Add("Content-Type", "application/json");
        //            multiPartContent.Add(byteArrayContent, "file", fileName);
        //            multiPartContent.Add(byteArrayContent, fileName, "filename");
        //            multiPartContent.Add(byteArrayContent, "application/pdf", "type");
        //            foreach (var keyValuePair in values)
        //            {
        //                multiPartContent.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
        //            }
        //            requestMessage.Content = multiPartContent;

        //            Task<HttpResponseMessage> httpRequest = httpClient.SendAsync(requestMessage);
        //            HttpResponseMessage httpResponse = httpRequest.Result;

        //            HttpStatusCode statusCode = httpResponse.StatusCode;
        //            HttpContent responseContent = httpResponse.Content;

        //            var result = responseContent.ReadAsStringAsync();

        //            XDocument doc = XDocument.Parse(result.Result);
        //            fileID = doc.Root.Descendants().Last().Value;
        //        }

        //        return fileID;
        //        #endregion                
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return null;
        //}
        #endregion

        #region GetConnectionString
        public static JObject GetConnectionString(string ExternalDataSourceID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetFilterQuery_Cases)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ExternalDataSourceID", ExternalDataSourceID.ToString())

            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Filter Query Cases
        public static JObject GetFilterQueryCases(string ExternalDataSourceID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetFilterQuery_Cases)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ExternalDataSourceID", ExternalDataSourceID.ToString())

            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Email Field
        public static JObject GetEmailField(string caseTypeId, string emailAddress)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetEmailField)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("caseTypeId", caseTypeId),
                new KeyValuePair<string, string>("emailAddress", emailAddress)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Case Basic Info
        public static JObject GetCaseBasicInfo(string user, string caseID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetCaseBasicInfo)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", user),
                new KeyValuePair<string, string>("caseID", caseID)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Create Case Optimized
        public static JObject CreateCaseOptimized(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.CreateCaseOptimized);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Case ID By CaseTypeID And ListID
        public static JObject GetCaseIDByCaseTypeIDAndListID(string caseTypeId, string listID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetCaseIDByCaseTypeIDAndListID)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("caseTypeId", caseTypeId),
                new KeyValuePair<string, string>("listID", listID)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Format Phone
        public static JObject FormatPhone(string phoneNumber)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.FormatPhone)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("caseTypeId", phoneNumber)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Subscribe To Case Alerts
        public static JObject SubscribeToCaseAlerts(string caseId, string caseTypeID, string emailAddress, string currentUser)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.SubscribeToCaseAlerts)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("caseId", caseId),
                new KeyValuePair<string, string>("caseTypeID", caseTypeID),
                new KeyValuePair<string, string>("emailAddress", emailAddress),
                new KeyValuePair<string, string>("currentUser", currentUser)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Prevous Case Assignment
        public static JObject GetPrevousCaseAssignment(string caseId)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetPrevousCaseAssignment)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("caseId", caseId)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Send Email Alert
        public static JObject SendEmailAlert(string recipent, string subject, string body)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.SendEmailAlert)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("recipent", recipent),
                new KeyValuePair<string, string>("subject", subject),
                new KeyValuePair<string, string>("body", body),
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Subscriptions
        public static JObject GetSubscriptions(string key)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetSubscriptions)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("key", key)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Types By CaseType
        public static JObject GetTypesByCaseType(string caseTypeId)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetTypesByCaseType)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("caseTypeId", caseTypeId)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Save Case
        public static JObject SaveCase(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.SaveCase);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Save Case Optimized
        public static JObject SaveCaseOptimized(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.SaveCaseOptimized);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Case Modified Date
        public static JToken GetCaseModifiedDate(int _caseid, DateTime _modifieddate)
        {
            try
            {
                GetCaseModifiedDateRequest Body_value = new GetCaseModifiedDateRequest();
                Body_value.caseID = _caseid;
                Body_value.LastSyncModifiedTime = _modifieddate;
                var Result = Constants.ApiCommon(Body_value, Constants.GetCaseModifiedDate);
                return Result.GetValue("ResponseContent");
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Accept Case
        public static JObject AcceptCase(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.AcceptCase);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Employees By Search
        public static JObject GetEmployeesBySearch(string user)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetEmployeesBySearch)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                 new KeyValuePair<string, string>("username", user),
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get All External Datasource List
        public static JObject GetAllExternalDatasourceList(string _CaseTypeID, string _AssocTypeID, string _user)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetAllExternalDatasourceList)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("CaseTypeID", _CaseTypeID),
                new KeyValuePair<string, string>("AssocTypeID", _AssocTypeID),
                 new KeyValuePair<string, string>("user", _user),
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Team Members
        public static JObject GetTeamMembers(string _ShortUserName)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetTeamMembers)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ShortUserName", _ShortUserName)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Favorite

        public static JObject GetFavorite(string _CreatedBy, string _ApplicationId)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetFavorite)
                //new KeyValuePair<string, string>("API_Name","http://localhost:54493/api/v1/Cases/GetFavoritesWIP")
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("CreatedBy", _CreatedBy),
                new KeyValuePair<string, string>("ApplicationId", _ApplicationId),
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }

        //public static JObject GetFavorite(string _CreatedBy)
        //{
        //    #region API Details
        //    var API_value = new List<KeyValuePair<string, string>>
        //    {
        //        new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetFavorite)
        //    };
        //    #endregion

        //    #region API Body Details
        //    var Body_value = new List<KeyValuePair<string, string>>
        //    {
        //        new KeyValuePair<string, string>("CreatedBy", _CreatedBy)
        //    };
        //    #endregion

        //    var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
        //    if (Result != null)
        //    {
        //        return Result;
        //    }
        //    else
        //        return null;
        //}
        #endregion

        #region Edit Favorite
        public static JObject EditFavorite(string _FavoriteID, string AppID, string _FavoriteName, string _FieldValues, string _IsActive, string _CreatedBy, string _CreatedByDt, string _ModifiedByDt, string _LastSyncDt, string QuestAreaID, string pTypeId)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.EditFavorite)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("FavoriteID", _FavoriteID),
                new KeyValuePair<string, string>("AppTypeInfoId", AppID),
                new KeyValuePair<string, string>("FavoriteName", _FavoriteName),
                new KeyValuePair<string, string>("FieldValues", _FieldValues),
                new KeyValuePair<string, string>("IsActive", _IsActive),
                new KeyValuePair<string, string>("CreatedBy", _CreatedBy),
                new KeyValuePair<string, string>("CreatedByDt", _CreatedByDt),
                new KeyValuePair<string, string>("ModifiedByDt", _ModifiedByDt),
                new KeyValuePair<string, string>("LastSyncDt", _LastSyncDt),
                   new KeyValuePair<string, string>("QuestAreaID", QuestAreaID),
                new KeyValuePair<string, string>("pTypeId", pTypeId)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Remove Favorite
        public static JObject RemoveFavorite(string _FavoriteID, string username)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.RemoveFavorite)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("FavoriteID", _FavoriteID),
                 new KeyValuePair<string, string>("username", username)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region DeleteFavorite
        public static JObject RemoveFavorite(string _FavoriteID, string _AppTypeInfoId, string _FavoriteName, string _FieldValues, string _IsActive, string _CreatedBy, string _CreatedByDt, string _ModifiedByDt, string _LastSyncDt)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.RemoveFavorite)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("FavoriteID", _FavoriteID)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Add Favorite
        public static JObject AddFavorite(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.AddFavorite);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Get External DataSource By Id
        public static JObject GetExternalDataSourceById(string _ExternalDatasourceID, string _SystemCode)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetExternalDataSourceById)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ExternalDatasourceID", _ExternalDatasourceID),
                new KeyValuePair<string, string>("SystemCode", _SystemCode)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Case Activity
        public static JObject GetCaseActivity(string _case_ID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetCaseActivity)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("case_ID", _case_ID)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get CalculationFields
        public static JObject RefreshCalculationFields(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.RefreshCalculationFields);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Types For List By CaseTypeID
        public static JObject GetTypesForListByCaseTypeID(string _caseTypeId, string _userName)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetTypesForListByCaseTypeID)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("caseTypeId", _caseTypeId),
                new KeyValuePair<string, string>("userName", _userName)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get ExternalDataSource Items By Id
        public static JObject GetExternalDataSourceItemsById(string _ExternalDatasourceID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetExternalDataSourceItemsById)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ExternalDatasourceID", _ExternalDatasourceID)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get ExternalDataSource Items By Id
        public static JObject GetItemValueFromQueryExd(string _connectionString, string _query)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetItemValueFromQueryExd)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("connectionString", _connectionString),
                new KeyValuePair<string, string>("query", _query)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region SubscribeToHopper
        public static JObject SubscribeToHopper(string pHopper, string pUser)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.SubscribeToHopper)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("pHopper", pHopper),
                 new KeyValuePair<string, string>("pUser", pUser)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region ReturnCaseToLastAssignee
        public static JObject ReturnCaseToLastAssignee(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.ReturnCaseToLastAssignee);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region ReturnCaseToLastAssignee
        public static JObject ReturnCaseToLastAssigner(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.ReturnCaseToLastAssigner);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetValuesQueryAndConnection
        public static JObject GetValuesQueryAndConnection(string caseTypeID, string assocCaseTypeID, string caseTypeDesc, string isRequired, string connectionString, string query)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetValuesQueryAndConnection)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("caseTypeID", caseTypeID),
                 new KeyValuePair<string, string>("assocCaseTypeID", assocCaseTypeID),
                  new KeyValuePair<string, string>("caseTypeDesc", caseTypeDesc),
                   new KeyValuePair<string, string>("isRequired", isRequired),
                    new KeyValuePair<string, string>("connectionString", connectionString),
                     new KeyValuePair<string, string>("query", query),

            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region GetAssocCascadeInfoByCaseType
        public static JObject GetAssocCascadeInfoByCaseType(string caseTypeID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetAssocCascadeInfoByCaseType)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("caseTypeID", caseTypeID)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region GetAssocCascadeInfoByCaseType
        public static JObject CasesHomeGetCaseListUser(string username)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.CasesHomeGetCaseListUser)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", username)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region DeclienAndReturn
        public static JObject DeclienAndReturn(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.DeclineAndReturn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region DeclienAndReturn
        public static JObject DeclienAndAssign(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, "/api/v1/Cases/DeclienAndAssign");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ApproveandReturn
        public static JObject ApproveandReturn(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.ApproveandReturn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ApproveandAssign
        public static JObject ApproveandAssign(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.ApproveandAssign);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get All Employee User  
        public static JObject GetAllEmployeeUser()
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetAllEmployeeUser)
            };
            #endregion            

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, new List<KeyValuePair<string, string>>(), "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region GetSearchData
        public static JObject GetSearchData(SearchRequest Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.GetSearchData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Search Data
        public static JObject GetSearchData(string SystemId, string TypeId, string FieldId, string ItemInfoFieldsId, string SearchText, string FromPageIndex, string ToPageIndex, string username)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetSearchData)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("SystemId", SystemId),
                 new KeyValuePair<string, string>("TypeId", TypeId),
                  new KeyValuePair<string, string>("FieldId", FieldId),
                   new KeyValuePair<string, string>("ItemInfoFieldsId", ItemInfoFieldsId),
                    new KeyValuePair<string, string>("SearchText", SearchText),
                     new KeyValuePair<string, string>("FromPageIndex", FromPageIndex),
                      new KeyValuePair<string, string>("ToPageIndex", ToPageIndex),
                      new KeyValuePair<string, string>("username", username),
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }


        #endregion

        #region EDS Cache
        public static JObject EDSCache(string EXTERNAL_DATASOURCE_ID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.EDSCache)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("EXTERNAL_DATASOURCE_ID", EXTERNAL_DATASOURCE_ID),
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }


        #endregion

        #region Get Cases For User
        public static JObject GetCasesForUser(string user, string assignedTo, string caseIds, string pageIndex, string pageSize, string sortBy, string sortOrder, string searchXml, string SystemCodes)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetCasesForUser)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("user", user),
                new KeyValuePair<string, string>("assignedTo", assignedTo),
                new KeyValuePair<string, string>("caseIds", caseIds),
                new KeyValuePair<string, string>("pageIndex", pageIndex),
                new KeyValuePair<string, string>("pageSize", pageSize),
                new KeyValuePair<string, string>("sortBy", sortBy),
                new KeyValuePair<string, string>("sortOrder", sortOrder),
                new KeyValuePair<string, string>("searchXml", searchXml),
                new KeyValuePair<string, string>("SystemCodes", SystemCodes),
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }


        #endregion

        #region GetCaseInfo
        public static JObject GetCaseInfo(string username, int CaseId)
        {
            try
            {
                GetCaseBasicInfoRequest Body_value = new GetCaseBasicInfoRequest();
                Body_value.username = username;
                Body_value.CaseId = CaseId;

                return Constants.ApiCommon(Body_value, Constants.GetCaseInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get File From Case
        public static JObject GetFileFromCase(string fileID, string userName)
        {
            //RequestModel
            GetCaseFileRequest getCaseFileModel = new GetCaseFileRequest();
            getCaseFileModel.FileID = fileID;
            getCaseFileModel.UserName = userName;

            return Constants.ApiCommon(getCaseFileModel, Constants.GetCaseFile);
        }
        #endregion
    }
}


