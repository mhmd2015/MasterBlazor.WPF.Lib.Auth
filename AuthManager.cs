
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

namespace MasterBlazor.WPF.Lib.Auth
{
    public class AuthManager
    {
        String filePath = "cookie.dat";
        public String siteUrl { get; set; }
        List<CoreWebView2Cookie> Cookies { get; set; }
        public AuthManager(string siteUrl)
        {
            this.siteUrl = siteUrl;
        }

        #region File
        public void Save()
        {
            //cant save in json because the tokens not well fitted

            if (Cookies == null) return;

            StringBuilder buffer = new StringBuilder();
            foreach (var cookie in Cookies)
            {
                buffer.AppendLine(cookie.Name);
                buffer.AppendLine(cookie.Value);
                buffer.AppendLine(cookie.Path);
                buffer.AppendLine(cookie.Domain);
            }
            File.WriteAllText(GetFileName(), buffer.ToString());
        }

        public CookieContainer Load()
        {
            filePath = GetFileName();
            if (!File.Exists(filePath))
                return null;

            var lines = File.ReadAllLines(filePath);
            if (lines == null || lines.Length == 0 || lines.Length % 4 != 0)
                return null;

            var cookieContainer = new CookieContainer();
            for (var i = 0; i < lines.Length; i += 4)
            {
                var cookie = new System.Net.Cookie(lines[i], lines[i + 1], lines[i + 2], lines[i + 3]);

                cookieContainer.Add(cookie);
                //Cookies.Add();


            }
            return cookieContainer;
        }
        public void Reset()
        {
            if (File.Exists(GetFileName()))
                File.Delete(GetFileName());
            if (Cookies != null)
                Cookies.Clear();
        }

        private String GetFileName()
        {
            if (siteUrl == "") return "cookies.data";

            string filePath = siteUrl.Replace("https://", "").Replace("http://", "");
            if (filePath.Contains("/"))
                filePath = filePath.Substring(0, filePath.IndexOf("/"));
            filePath = filePath.Replace(".", "_");
            return filePath + ".data";
        }

        #endregion

        #region Auth
        public string GetCookie(string cookieName = "FedAuth")
        {
            string token = "";
            if (Cookies == null) return token;

            foreach (var cookie in Cookies)
            {
                if (cookie.Name == cookieName) // Replace with your token's name
                {
                    token = cookie.Value;

                    break;
                }
            }
            return token;

        }
        public async Task<List<CoreWebView2Cookie>> Initialize(WebView2 webView)
        {
            string token = "";
            CoreWebView2 coreWebView2 = webView.CoreWebView2;

            var cookieManager = coreWebView2.CookieManager;

            Cookies = await cookieManager.GetCookiesAsync(siteUrl); // Specify your site's URL

            return Cookies;

        }
        #endregion
    }
}
