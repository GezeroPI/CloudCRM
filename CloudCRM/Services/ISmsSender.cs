using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudCRM.Services
{
    interface ISmsSender
    {
        bool SendSMS(string to, string message, string originator);
    }
}
