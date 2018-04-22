using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.DocumentDB;
using Microsoft.Azure.Documents;
using Twilio;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

namespace FunctionApp2
{
    public static class Func1
    {
        [FunctionName("CosmosQueueStore")]
        public static void Run(
            [CosmosDBTrigger(databaseName: "traffic", collectionName: "vehicles", ConnectionStringSetting = "DBConnection", LeaseCollectionName = "leases")]IReadOnlyList<Document> input,
            [Queue("trafficqueue", Connection = "AzureWebJobsStorage")] out string queueMessage, TraceWriter log)
        {
            queueMessage = string.Empty;
            if (input != null && input.Count > 0)
            {
                log.Verbose("Documents modified " + input.Count);
                log.Verbose("First document Id " + input[0].Id);
                string vehicleNo = input[0].GetPropertyValue<string>("vehicleNumber");
                double speed = input[0].GetPropertyValue<double>("speed");
                string city = input[0].GetPropertyValue<string>("city");
                string mobile = input[0].GetPropertyValue<string>("mobile");
                TrafficMessage trafficMessage = new TrafficMessage { VehicleNo = vehicleNo, City = city, Mobile = mobile, Speed = speed };
                if (speed > 80)
                {
                    string message = string.Format("High speed detected in {0}, Vehicle No {1} and Speed {2},", trafficMessage.City, trafficMessage.VehicleNo, trafficMessage.Speed);
                    queueMessage = message;
                    log.Verbose(message);

                }
            }
            }
    }
}
