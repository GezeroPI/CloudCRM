using System;
using Xunit;
using CloudCRM;
using CloudCRM.Models;
using CloudCRM.Services;

namespace EmailSenderTests
{
    public class SmsSenderTests
    {
        private SmsSender smsSender = new SmsSender("54e389c0c95128");
        
        [Theory]
        [InlineData("6940149917", "This is a test from unit test with xUnit", "CloudCRM")]
        public void SendSmsMethod_TestSms_ReturnsTrue(string to, string message, string originator)
        {
            var result = smsSender.SendSMS(to, message, originator);
            Assert.True(result);
        }
        
    }
}
