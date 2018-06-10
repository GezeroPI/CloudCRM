using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace CloudCRM.Services
{
    public class SmsSender : ISmsSender
    {
        private string _key { get; set; }
        

        public SmsSender(string key)
        {
            _key = key;
        }

        public bool SendSMS(string to, string message, string originator)
        {
            try
            {
                string url = "https://smscenter.gr/api/sms/send?key=" + _key +
                    "&to=" + to + "&text=" + HttpUtility.UrlEncode(message) +
                    "&from=" + HttpUtility.UrlEncode(originator);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream resStream = response.GetResponseStream();

                if (response.StatusDescription == "OK")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Registration Failure : {ex.Message} ");
                return false;
            }
            
        }
    }
}
