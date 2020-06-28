using System;
using System.Net.Http;
using System.Threading.Tasks;
using MercadoPagoCertQR.Models;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MercadoPagoCertQR.Notification
{
    public static class NotificationProcess
    {
        [FunctionName(nameof(NotificationProcess))]
        public static async Task Run(
            [QueueTrigger("notifications")] string id,
            [Table("qrorders")] CloudTable cloudTable,
            ILogger log)
        {
            HttpResponseMessage httpResponseMessage;
            using (var httpClient = new HttpClient())
            {
                httpResponseMessage = await httpClient.GetAsync($"https://api.mercadopago.com/merchant_orders/{id}?access_token={Environment.GetEnvironmentVariable("PROD_ACCESS_TOKEN")}");
            }

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var json = await httpResponseMessage.Content.ReadAsStringAsync();

                dynamic merchantOrder =
                    JsonConvert.DeserializeObject<dynamic>(json);

                await cloudTable.ExecuteAsync(TableOperation.InsertOrReplace(new MerchantOrder
                {
                    RowKey = merchantOrder.external_reference,
                    Json = json,
                    PartitionKey = "qr"
                }));
            }
            else
            {
                throw new ApplicationException();
            }
        }
    }
}