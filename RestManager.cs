using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MasterBlazor.WPF.Lib.Auth
{
    public class RestManager : IDisposable
    {
        HttpClientHandler handler { get; set; }
        public HttpClient httpClient { get; set; }

        public RestManager(AuthManager authManager)
        {
            if (!String.IsNullOrEmpty(authManager.siteUrl))
            {
                Initialize(authManager.Load(), siteUrl: authManager.siteUrl);
            }
        }
        public RestManager(CookieContainer cookieContainer, string siteUrl)
        {
            Initialize(cookieContainer, siteUrl);
        }

        void Initialize(CookieContainer cookieContainer, string siteUrl)
        {
            handler = new HttpClientHandler();
            if (cookieContainer != null)
                handler.CookieContainer = cookieContainer;

            httpClient = new HttpClient(handler);
            httpClient.BaseAddress = new Uri(siteUrl);
        }

        public async Task<string> Get(string api)
        {
            // Make the API request (replace with your actual API endpoint)
            HttpResponseMessage response = await httpClient.GetAsync(api);

            if (response.IsSuccessStatusCode)
            {
                // Process the API response
                string content = await response.Content.ReadAsStringAsync();
                // ... (Use the retrieved data)
                return content;
            }
            else
            {
                // Handle the error response
                // ...
                return "Error: call was not success";
            }
        }

        public void Reset()
        {
            httpClient.Dispose();
            handler.Dispose();
        }

        public void Dispose()
        {
            Reset();
        }
    }
}
