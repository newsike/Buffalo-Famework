using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace Buffalo.Basic.Net
{
    class WebProgress
    {
        public WebProgress()
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 10000;
        }

        public byte[] getRemoteRequestToByte(string input, string remoteurl, int requestTimeOut, int buffersize, List<Cookie> activeCookies, string activeParameterName)
        {
            try
            {
                if (activeParameterName != "")
                    input = activeParameterName + "=" + input;
                byte[] bytes = Encoding.Default.GetBytes(input);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(remoteurl);
                request.CookieContainer = new CookieContainer();
                if (activeCookies != null && activeCookies.Count > 0)
                    foreach (Cookie activeCookie in activeCookies)
                        request.CookieContainer.Add(activeCookie);
                request.Method = "post";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Flush();
                requestStream.Close();
                Stream responseStream = ((HttpWebResponse)request.GetResponse()).GetResponseStream();
                byte[] buffer2 = null;
                BinaryReader reader = new BinaryReader(responseStream);
                buffer2 = reader.ReadBytes(buffersize);
                reader.Close();
                responseStream.Close();
                return buffer2;
            }
            catch
            {
                return null;
            }
        }

        public byte[] getRemoteRequestToByte(string remoteurl, int requestTimeOut, int buffersize, List<Cookie> activeCookies)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(remoteurl);
                request.CookieContainer = new CookieContainer();
                if (activeCookies != null && activeCookies.Count > 0)
                    foreach (Cookie activeCookie in activeCookies)
                        request.CookieContainer.Add(activeCookie);
                request.Method = "get";
                Stream responseStream = ((HttpWebResponse)request.GetResponse()).GetResponseStream();
                byte[] buffer2 = null;
                BinaryReader reader = new BinaryReader(responseStream);
                buffer2 = reader.ReadBytes(buffersize);
                reader.Close();
                responseStream.Close();
                return buffer2;
            }
            catch
            {
                return null;
            }
        }

        public string getRemoteRequestToString(string remoteurl, List<Cookie> activeCookies)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(remoteurl);
                request.CookieContainer = new CookieContainer();
                if (activeCookies != null && activeCookies.Count > 0)
                    foreach (Cookie activeCookie in activeCookies)
                        request.CookieContainer.Add(activeCookie);
                request.Method = "get";
                Stream responseStream = ((HttpWebResponse)request.GetResponse()).GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string str = reader.ReadToEnd();
                reader.Close();
                responseStream.Close();
                return str;
            }
            catch
            {
                return "";
            }
        }

        public string getRemoteRequestToString(string input, string remoteurl, List<Cookie> activeCookies)
        {
            try
            {
                byte[] bytes = Encoding.Default.GetBytes(input);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(remoteurl);
                request.CookieContainer = new CookieContainer();
                if (activeCookies != null && activeCookies.Count > 0)
                    foreach (Cookie activeCookie in activeCookies)
                        request.CookieContainer.Add(activeCookie);
                request.Method = "post";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Flush();
                requestStream.Close();
                Stream responseStream = ((HttpWebResponse)request.GetResponse()).GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string str = reader.ReadToEnd();
                reader.Close();
                responseStream.Close();
                return str;
            }
            catch
            {
                return "";
            }
        }

        public List<Cookie> getRemoteServerCookie(string remoteurl, string input,string activePatamterName)
        {
            try
            {
                List<Cookie> result = new List<Cookie>();
                HttpWebResponse response = null;
                HttpWebRequest request = null;
                Stream requestStream = null;
                byte[] bytes = Encoding.Default.GetBytes(input);
                request = (HttpWebRequest)WebRequest.Create(remoteurl);
                CookieContainer cookies = new CookieContainer();
                request.Method = "post";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                request.CookieContainer = cookies;
                requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Flush();
                response = (HttpWebResponse)request.GetResponse();
                Uri newUri = new Uri(remoteurl);
                if (response != null && request != null && requestStream != null && response.StatusCode == HttpStatusCode.OK && request.CookieContainer.GetCookies(newUri).Count > 0)
                {
                    List<string> tmpstrCookiesList = new List<string>();
                    foreach (Cookie activeCookie in request.CookieContainer.GetCookies(newUri))
                    {
                        tmpstrCookiesList.Add(activeCookie.Domain + ":" + activeCookie.Name + "=" + activeCookie.Value);
                        result.Add(activeCookie);
                    }
                    requestStream.Close();
                }
                return result;
            }
            catch
            {
                return null;
            }
        }

    }
}
