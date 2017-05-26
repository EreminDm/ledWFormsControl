using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ledWFormsControl
{
    public class Server
    {
        public void SendRequest(object postData)
        {

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://url");

            string postDataJSON = postData.ToString();
            // postData += "&thing2=world";
            var Data = Encoding.ASCII.GetBytes(postDataJSON);


            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.ContentLength = Data.Length;

            using (var stream = httpWebRequest.GetRequestStream())
            {
                stream.Write(Data, 0, Data.Length);
            }

            var response = (HttpWebResponse)httpWebRequest.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
    }
}
