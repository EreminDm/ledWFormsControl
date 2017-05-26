using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static ledWFormsControl.Form1;

namespace ledWFormsControl
{
    public class Server
    {
        
        public void SendRequest(object sendingData)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://url");

            string postDataJSON = JsonConvert.SerializeObject(sendingData);
            //var Data = Encoding.ASCII.GetBytes(postDataJSON);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                streamWriter.Write(postDataJSON);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var response = (HttpWebResponse)httpWebRequest.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
    }
}
