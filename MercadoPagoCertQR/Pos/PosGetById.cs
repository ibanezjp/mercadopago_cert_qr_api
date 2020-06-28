using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace MercadoPagoCertQR.Pos
{
    public static class PosGetById
    {
        [FunctionName(nameof(PosGetById))]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "pos/{id}")] HttpRequest req,
            string id,
            ILogger log)
        {
            HttpResponseMessage httpResponseMessage;
            using (var httpClient = new HttpClient())
            {
                httpResponseMessage = await httpClient.GetAsync($"https://api.mercadopago.com/pos/{id}?access_token={Environment.GetEnvironmentVariable("PROD_ACCESS_TOKEN")}");
            }
            return new HttpResponseMessage(httpResponseMessage.StatusCode)
            {
                Content = new StringContent(await httpResponseMessage.Content.ReadAsStringAsync(), Encoding.UTF8, "application/json")
            };
        }
    }
}