using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace GoogleLoginDemo.Helpers
{
    public static class HttpHelper
    {
        public static string CreateHttpGetRequest(string apiUrl, string data)
        {
            string result = string.Empty;

            WebRequest request = WebRequest.Create(apiUrl);
            request.Method = "POST";
            request.Timeout = 3000;
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            request.ContentLength = byteArray.Length;

            // 將需 post 的資料內容轉為 stream 
            using (StreamWriter sw = new StreamWriter(request.GetRequestStream()))
            {
                sw.Write(data);
                sw.Flush();
            }

            // 使用 GetResponse 方法將 request 送出
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8))
                    {
                        result = sr.ReadToEnd();
                    }
                }
            }

            return result;
        }
    }
}