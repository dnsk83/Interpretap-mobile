﻿using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Interpretap.Services
{
    public class BaseServiceNoNulls : BaseService
    {
        protected async Task<TResult> PostNoNulls<TResult, TData>(string endPoint, TData data) where TData : class where TResult : class
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, endPoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            var jsonRequest = JsonConvert.SerializeObject(data, serializerSettings);
            request.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.SendAsync(request);
            string result = await response.Content.ReadAsStringAsync();
            var e = JsonConvert.DeserializeObject<TResult>(result);
            return e;
        }
    }
}