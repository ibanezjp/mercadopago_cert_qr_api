using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace MercadoPagoCertQR.Notification
{
    public static class NotificationCreate
    {
        [FunctionName(nameof(NotificationCreate))]
        [return:Queue("notifications")]
        public static string Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "notification/create")] HttpRequest httpRequest,
            ILogger log)
        {
            if (httpRequest.Query.ContainsKey("id") && 
                httpRequest.Query.ContainsKey("topic") && 
                httpRequest.Query["topic"].ToString().EndsWith("merchant_order", StringComparison.InvariantCultureIgnoreCase))
            {
                return httpRequest.Query["id"].ToString();
            }
            return null;
        }
    }
}