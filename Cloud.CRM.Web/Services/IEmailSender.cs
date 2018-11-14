using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloud.CRM.Web.Models
{
    public interface IEmailSender
    {
        bool SendMail(string emailTo, string subject, string message);
    }
}
