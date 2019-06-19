using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsAnalyse.Services.EmailSenders
{
    public static class EmailMessageGenerator
    {
        public static string GenerateEmailConfirmMessage(string path)
        {
              string message ="Для того, щоб здійснювати розширенні дії відносно нашого сайту \n";
                    message += "Для цього перейдіть по посиланню знизу: \n";
                    message += "<a href = "+path+"> Підтвердити</a>";
            return message;
        }
    }
}
