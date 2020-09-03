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
                "&client_id=" + googleClientId +
                "&prompt=select_account";

            return Redirect(oauthUrl);
        }

        public RedirectResult LoginHandler()
        {
            var url = Request.Url.Query;
            if (url != "")
            {
                string code = Request.QueryString["code"];

                if (code != null)
                {
                    var result = GoogleLoginService.GetGoogleOauth(code);

                    Session["AccessToken"] = result.token;
                    Session["UserId"] = result.id;
                    Session["UserEmail"] = result.email;
                    Session["UserGivenName"] = result.given_name;
                }
            }

            TempData["message"] = "登入成功";

            return Redirect(Url.Action("LoginComplete"));
        }

        public ActionResult LoginComplete()
        {
            return View();
        }

        public RedirectResult Logout()
        {
            if (Session["AccessToken"] != null && GoogleLoginService.OuathChecker(Session["AccessToken"].ToString()))
            {
                HttpHelper.CreateHttpGetRequest("https://accounts.google.com/o/oauth2/revoke?token=" + Session["AccessToken"], "");
            }

            TempData["message"] = "登出成功";
            return Redirect(Url.Action("Index", "Home"));

            //return Redirect(string.Format("https://accounts.google.com/o/oauth2/revoke?token={0}", Session["AccessToken"]));
            //return Redirect("https://www.google.com/accounts/Logout?continue=https://appengine.google.com/_ah/logout?continue=" + Url.Action("Index", "Home", null, Request.Url.Scheme));
        }
    }
}