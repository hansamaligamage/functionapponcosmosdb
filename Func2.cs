using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;

namespace FunctionApp2
{
   public static class Function3
    {

        [FunctionName("QueueTwilio")]
        [return: TwilioSms(AccountSidSetting = "AccountSid", 
            AuthTokenSetting = "AuthToken", From = "+15702588933")]
        public static SMSMessage Run([QueueTrigger("trafficqueue", 
            Connection = "AzureWebJobsStorage")] string text, TraceWriter log)
        {
            log.Info($"C# Queue trigger function processed: {text}");
            var message = new SMSMessage()
            {
                Body = string.Format("Drive focused, Drive smart - {0}", text),
                To = "+94713355704"
            };
            return message;
        }

    }
}
