using DataServiceBus.OnlineHelper.DataTypes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Diagnostics;

namespace DataServiceBus
{
    public class MobileAPIMethods
    {
        //static HttpClient client = new HttpClient();

        #region Request for Token
        public static string RequestToken(string BaseUrl)
        {
            RequestToken rt = new RequestToken();
            HttpRequestMessage httpContent;
            Task<HttpResponseMessage> response;
            string accessToken = string.Empty;

            try
            {
                #region API Details
                var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", BaseUrl)
            };

                #endregion

                #region API Body Details
                var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type",rt.grant_type),
                new KeyValuePair<string, string>("username", rt.username),
                new KeyValuePair<string, string>("password", rt.password)
            };
                #endregion

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.Timeout = new TimeSpan(0, 15, 0);

                    httpContent = new HttpRequestMessage(HttpMethod.Post, BaseUrl);
                    httpContent.Headers.ExpectContinue = false;

                    httpContent.Content = new FormUrlEncodedContent(Body_value);
                    response = httpClient.SendAsync(httpContent);
                    response.Wait();
                    var result = response.Result.Content.ReadAsStringAsync();
                    JObject results = JObject.Parse(result.Result);
                    if (results != null)
                    {
                        ResponseToken responsejson = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseToken>(response.Result.Content.ReadAsStringAsync().Result);
                        accessToken = responsejson.access_token;
                    }
                }

                return accessToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Set API Get Post Method For Class Collection Parameter
        public static JObject CallAPIGetPostList<T>(List<KeyValuePair<string, string>> APIDetails, T ClassObject)
        {
            JObject jobj = null;
            try
            {
                var ObjectSerialize = JsonConvert.SerializeObject(ClassObject, Formatting.Indented);
                var ObjectDeserialize = JsonConvert.DeserializeObject(ObjectSerialize).ToString();

                string url = APIDetails[0].Value;
                string _ContentType = "application/json";

                string baseurl = url.Split(new string[] { "/api" }, StringSplitOptions.None)[0];

                HttpRequestMessage httpContent;
                Task<HttpResponseMessage> response;

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.Timeout = new TimeSpan(1, 10, 0);
                    string accessToken = RequestToken(baseurl + Constants.Get_Token);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_ContentType));

                    httpContent = new HttpRequestMessage(HttpMethod.Post, url);
                    httpContent.Headers.ExpectContinue = false;
                    HttpContent contentPost = new StringContent(ObjectSerialize, Encoding.UTF8, "application/json");

                    httpContent.Content = contentPost;

                    response = httpClient.SendAsync(httpContent);
                    var result = response.Result.Content.ReadAsStringAsync();
                    jobj = JObject.Parse(result.Result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jobj;
        }
        #endregion

        #region Set API Get Post Method For String Collection Parameter
        public static JObject CallAPIGetPost(List<KeyValuePair<string, string>> APIDetails, List<KeyValuePair<string, string>> BodyValue, string Method)
        {
            JObject jobj = null;
            try
            {
                string url = APIDetails[0].Value;
                string _ContentType = "application/json";

                string baseurl = url.Split(new string[] { "/api" }, StringSplitOptions.None)[0];

                HttpRequestMessage httpContent;
                Task<HttpResponseMessage> response;

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.Timeout = new TimeSpan(1, 10, 0);
                    string accessToken = RequestToken(baseurl + Constants.Get_Token);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_ContentType));
                    if (Method.ToUpper() == "POST")
                    {
                        httpContent = new HttpRequestMessage(HttpMethod.Post, url);
                        httpContent.Headers.ExpectContinue = false;
                        httpContent.Content = new FormUrlEncodedContent(BodyValue);
                    }
                    else
                    {
                        httpContent = new HttpRequestMessage(HttpMethod.Get, url);
                    }
                    response = httpClient.SendAsync(httpContent);
                    var result = response.Result.Content.ReadAsStringAsync();
                    jobj = JObject.Parse(result.Result);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("URL => " + APIDetails[0].Value);
                Debug.WriteLine("InnerException => " + e.InnerException);
                Debug.WriteLine("Message => " + e.Message);
                throw e;
            }
            return jobj;
        }
        #endregion
    }
}