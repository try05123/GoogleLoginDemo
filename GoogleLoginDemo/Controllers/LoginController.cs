using GoogleLoginDemo.Helpers;
using GoogleLoginDemo.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GoogleLoginDemo.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public RedirectResult Index()
        {
            var googleClientId = ConfigurationManager.AppSettings["google_client_id"];
            var googleRedirectUrl = ConfigurationManager.AppSettings["google_redirect_url"];

            var oauthUrl = "https://accounts.google.com/o/oauth2/v2/auth?" +
                "response_type=code" +
                "&redirect_uri=" + googleRedirectUrl +
                "&scope=https://www.googleapis.com/auth/userinfo.email%20https://www.googleapis.com/auth/userinfo.profile" +
                "&client_id=" + googleClientId;

            return Redirect(oauthUrl);
        }

        public RedirectResult LoginHandler()
        {
            try
            {
                var url = Request.Url.Query;
                if (url != "")
                {
                    string code = Request.QueryString["code"];

                    if (code != null)
                    {
                        var result = GoogleLoginService.GetGoogleOauth(code);

                        Session["UserId"] = result.id;
                        Session["UserEmail"] = result.email;
                        Session["UserGivenName"] = result.given_name;
                    }
                }

                return Redirect(Url.Action("LoginComplete"));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public ActionResult LoginComplete()
        {
            return View();
        }
    }
}