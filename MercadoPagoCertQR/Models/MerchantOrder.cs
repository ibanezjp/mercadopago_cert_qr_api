using Microsoft.Azure.Cosmos.Table;

namespace MercadoPagoCertQR.Models
{
    public class MerchantOrder : TableEntity
    {
        public string Json { get; set; }
    }
}
