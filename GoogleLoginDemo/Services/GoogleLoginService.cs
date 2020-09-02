using GoogleLoginDemo.Helpers;
using GoogleLoginDemo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace GoogleLoginDemo.Services
{
    public static class GoogleLoginService
    {
        private static readonly string getTokenUrl = "https://accounts.google.com/o/oauth2/token";
        private static readonly string urlProfile = "https://www.googleapis.com/oauth2/v1/userinfo?access_token=";
        private static readonly string googleClientId = ConfigurationManager.AppSettings["google_client_id"];
        private static readonly string googleClientSecret = ConfigurationManager.AppSettings["google_client_secret"];
        private static readonly string googleRedirectUrl = ConfigurationManager.AppSettings["google_redirect_url"];

        public static GoogleUserOutputData GetGoogleOauth(string code)
        {
            var data = $"code={code}&client_id={googleClientId}&client_secret={googleClientSecret}&redirect_uri={googleRedirectUrl}&grant_type=authorization_code";
            var getTokenCode = HttpHelper.CreateHttpGetRequest(getTokenUrl, data);

            GoogleUserOutputData result = null;
            GoogleAccessToken tokenInfo = JsonConvert.DeserializeObject<GoogleAccessToken>(getTokenCode);

            if (tokenInfo != null)
            {
                string accessToken = tokenInfo.access_token;
                if (!string.IsNullOrWhiteSpace(accessToken))
                {
                    WebClient client = new WebClient();
                    client.Encoding = Encoding.UTF8;

                    var url = urlProfile + accessToken;
                    string downloadString = client.DownloadString(url);

                    result = JsonConvert.DeserializeObject<GoogleUserOutputData>(downloadString);

                    result.token = accessToken;
                }
            }

            return result;
        }

        public static bool OuathChecker(string accessToken)
        {
            try
            {
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;

                var url = urlProfile + accessToken;
                client.DownloadString(url);

                return true;
            }
            catch (WebException e)
            {
                return false;
            }
        }
    }
}