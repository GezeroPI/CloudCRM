using System;
using Xunit;
using CloudCRM;
using CloudCRM.Models;
using CloudCRM.Services;

namespace EmailSenderTests
{
    public class EmailSenderTests
    {
        private EmailSender emailSender = new EmailSender("info@projectnow.gr", "HDLmQ8blgh5c", "mail.projectnow.gr");
        
        [Theory]
        [InlineData("info@ndh-webstudio.com", "test", "this is a test message")]
        public void SendMailMethod_TestMail_ReturnsTrue(string emailTo, string subject, string message)
        {
            var result = emailSender.SendMail(emailTo, subject, message);
            Assert.True(result);
        }
        
    }
}
