using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;


namespace Cloud.CRM.Web.Models
{
    public class EmailSender : IEmailSender
    {
        private string _username { get; set; }
        private string _password { get; set; }
        private string _outserver { get; set; }

        public EmailSender(string usernameSMTP, string passwordSMTP, string outgoingServer)
        {
            _username = usernameSMTP;
            _password = passwordSMTP;
            _outserver = outgoingServer;
        }

        public bool SendMail(string emailTo, string subject, string message)
        {
            try
            {
                //making connection
                SmtpClient client = new SmtpClient(_outserver);
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_username, _password);
                //"build" & send the message
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("info@projectnow.gr");
                mailMessage.To.Add(emailTo);
                mailMessage.Body = message;
                mailMessage.Subject = subject;
                client.Send(mailMessage);

                return true;
            }
            catch (SmtpException)
            {
                return false;
            }
        }
        
    }
}
