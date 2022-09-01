using Model.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace ViewModel
{
    public class Connect
    {
        public static HttpResponseMessage Get(string url, IModel model)
        {
            var httpClient = new HttpClient(new HttpClientHandler())
            {
                BaseAddress = new Uri(url),
                Timeout = new TimeSpan(0, 0, 6)
            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };
            if (model != null)
            {
                request.Content = new StringContent(JsonSerializer.Serialize(model, model.GetType()), Encoding.UTF8, "application/json");
            }

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes("NETSOF:NETSOF1234");
            string val = System.Convert.ToBase64String(plainTextBytes);
            request.Headers.Add("Authorization", "Basic " + val);

            return httpClient.SendAsync(request).Result;
        }
    }
}
