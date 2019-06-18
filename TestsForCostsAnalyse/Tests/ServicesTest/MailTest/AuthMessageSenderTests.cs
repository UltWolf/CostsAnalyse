using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TestsForCostsAnalyse.Tests.ServicesTest.MailTest
{
    public class AuthMessageSenderTests
    {
        AuthMessageSender authMessage = new AuthMessageSender();
        [Fact]
        public async void IsSending()
        {
             
            await authMessage.SendEmailAsync("ultwolf@gmail.com", "blah", "blah-blah-blah");
        }
    }
}
