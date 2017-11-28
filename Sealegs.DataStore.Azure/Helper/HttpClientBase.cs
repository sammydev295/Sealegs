using System;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;

using Newtonsoft.Json;
using System.Threading.Tasks;
using Sealegs.Clients.Portable;

namespace Sealegs.DataStore.Azure
{
    public class HttpClientBase
    {
        #region Constants & Statics
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings();
        private const int MaxRetryCount = 5;
        #endregion

        #region Properties
        protected string ApiBase { get; set; } = Constants.APIBase;
        #endregion

        #region GetServicePathUri

        protected Uri GetServicePathUri(string path)
        {
            return new Uri(new Uri(ApiBase), path);
        }

        #endregion

        #region Http Requests

        #region Get

        public async Task<T> HttpGetRequest<T>(String requestPath)
        {
            return await HttpGetRequest<T>(requestPath, null);
        }

        public async Task<T> HttpGetRequest<T>(String requestPath, AuthenticationHeaderValue header)
        {
            string jsonString = string.Empty;

            try
            {
                var data = await HttpGetRequestInternal(requestPath, header);
                for (int i = 0; i < MaxRetryCount && data.Item1 == false; i++)
                {
                    data = await HttpGetRequestInternal(requestPath, header);
                }
                jsonString = data.Item2;

                return JsonConvert.DeserializeObject<T>(jsonString, Settings);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in deserialization of: [{jsonString}]: {ex.Message}", ex);
            }
        }

        #region Get Root Call

        protected async Task<Tuple<bool, string>> HttpGetRequestInternal(String requestPath, AuthenticationHeaderValue header)
        {
            using (var client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(Addresses.Token))
                {
                    client.DefaultRequestHeaders.Add("zumo-api-version", "2.0.0");
                    client.DefaultRequestHeaders.Add("X-ZUMO-AUTH", Addresses.Token); 
                }
                
                HttpResponseMessage response = await client.GetAsync(GetServicePathUri(requestPath));
                
                string jsonString;
                try
                {
                    jsonString = await response.Content.ReadAsStringAsync();
                    return new Tuple<bool, string>(true, jsonString);

                }
                catch (Exception ex)
                {
                    jsonString = ex.Message;
                }
                return new Tuple<bool, string>(false, jsonString);
            }
        }

        #endregion

        #endregion

        #region Post

        public async Task<T> HttpPostRequest<T>(String requestPath, Dictionary<string, string> form)
        {
            return await HttpPostRequest<T>(requestPath, null, form);
        }

        public async Task<T> HttpPostRequest<T>(String requestPath, AuthenticationHeaderValue header, Dictionary<string, string> form)
        {
            string jsonString = string.Empty;

            try
            {
                var data = await HttpPostRequestInternal(requestPath, header, form);
                for (int i = 0; i < MaxRetryCount && data.Item1 == false; i++)
                {
                    data = await HttpGetRequestInternal(requestPath, header);
                }
                jsonString = data.Item2;

                var result = JsonConvert.DeserializeObject<T>(jsonString, Settings);

                return result;
            }
            catch (Exception e)
            {
                throw new Exception($"Error in deserialization of: [{jsonString}]: {e.Message}", e);
            }
        }

        #region POST Root Call

        protected async Task<Tuple<bool, string>> HttpPostRequestInternal(String requestPath,
            AuthenticationHeaderValue header, Dictionary<string, string> form)
        {
            using (var client = new HttpClient())
            {
                //if (header != null)
                //    client.DefaultRequestHeaders.Authorization = header;
                //else
                //{
                //    client.DefaultRequestHeaders.Add("X-ZUMO-AUTH", Api.ApiBase.Token);
                //}
                if (!string.IsNullOrEmpty(Addresses.Token))
                {
                    client.DefaultRequestHeaders.Add("zumo-api-version", "2.0.0");
                    client.DefaultRequestHeaders.Add("X-ZUMO-AUTH", Addresses.Token);
                }

                HttpResponseMessage response = await client.PostAsync(GetServicePathUri(requestPath), new FormUrlEncodedContent(form));
                string jsonString = String.Empty;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    try
                    {
                        jsonString = await response.Content.ReadAsStringAsync();
                        return new Tuple<bool, string>(true, jsonString);
                    }
                    catch (Exception ex)
                    {
                        jsonString = ex.Message;
                    }
                }
                return new Tuple<bool, string>(false, jsonString);
            }
        }

        #endregion

        #endregion

        #endregion

        #region API Request Headers 

        public AuthenticationHeaderValue LoginApiHeader()
        {
            var authorizationHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes("xyz:secretKey"));
            return new AuthenticationHeaderValue("Basic", authorizationHeader);
        }

        //public HttpHeaders PostLoginApiHeader()
        //{

        //    return new AuthenticationHeaderValue();
        //}
        #endregion
    }
}
