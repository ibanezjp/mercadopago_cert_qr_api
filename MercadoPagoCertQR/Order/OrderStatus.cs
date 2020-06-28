using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace MercadoPagoCertQR.Order
{
    public static class OrderStatus
    {
        [FunctionName(nameof(OrderStatus))]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "order/status/{externalReference}")] HttpRequest req,
            string externalReference,
            ILogger log)
        {
            HttpResponseMessage httpResponseMessage;
            using (var httpClient = new HttpClient())
            {
                httpResponseMessage = await httpClient.GetAsync($"https://api.mercadopago.com/merchant_orders?external_reference={externalReference}&access_token={Environment.GetEnvironmentVariable("PROD_ACCESS_TOKEN")}");
            }
            return new HttpResponseMessage(httpResponseMessage.StatusCode)
            {
                Content = new StringContent(await httpResponseMessage.Content.ReadAsStringAsync(), Encoding.UTF8, "application/json")
            };
        }
    }
}