using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Threading;


namespace ledWFormsControl
{
    public class Server
    {
        
        public void SendRequest(object sendingData)
        {
            var date = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
            using (StreamWriter file = File.CreateText(@"C:\senderLog\log"+date+".txt"))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, sendingData);
            }

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.2.108:8080");

            string postDataJSON = JsonConvert.SerializeObject(sendingData);
            var Data = Encoding.ASCII.GetBytes(postDataJSON);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                streamWriter.Write(Data);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var response = (HttpWebResponse)httpWebRequest.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            Thread.Sleep(15000);
            Application.Exit();

        }
    }
}
