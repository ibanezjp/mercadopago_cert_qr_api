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
    public static class PosCreate
    {
        [FunctionName(nameof(PosCreate))]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "pos/create")] HttpRequest req,
            ILogger log)
        {
            HttpResponseMessage httpResponseMessage;
            using (var httpClient = new HttpClient())
            {
                var httpContent = new StreamContent(req.Body);
                httpContent.Headers.Add("Content-Type", "application/json");
                httpResponseMessage = await httpClient.PostAsync($"https://api.mercadopago.com/pos?access_token={Environment.GetEnvironmentVariable("PROD_ACCESS_TOKEN")}", httpContent);
            }
            return new HttpResponseMessage(httpResponseMessage.StatusCode)
            {
                Content = new StringContent(await httpResponseMessage.Content.ReadAsStringAsync(), Encoding.UTF8, "application/json")
            };
        }
    }
}