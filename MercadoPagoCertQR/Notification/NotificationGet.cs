using System.Net;
using System.Net.Http;
using System.Text;
using MercadoPagoCertQR.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace MercadoPagoCertQR.Notification
{
    public static class NotificationGet
    {
        [FunctionName(nameof(NotificationGet))]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "notification/get/{id}")] HttpRequest httpRequest,
            [Table("qrorders", "qr", "{id}")] MerchantOrder merchantOrder,
            ILogger log)
        {
            if (merchantOrder != null)
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(merchantOrder.Json, Encoding.UTF8, "application/json")
                };
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }
    }
}