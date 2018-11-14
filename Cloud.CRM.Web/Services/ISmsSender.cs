using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloud.CRM.Web.Services
{
    interface ISmsSender
    {
        bool SendSMS(string to, string message, string originator);
    }
}
