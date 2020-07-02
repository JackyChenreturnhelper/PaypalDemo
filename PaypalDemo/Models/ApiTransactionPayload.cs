using PaypalDemo.Models.test.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models
{
    public class ApiTransactionPayload : CreateOnlyPayload
    {
        public long apiTransactionId { get; set; }

        public string transactionNumber { get; set; }

        public string currencyCode { get; set; }

        public string transactionType { get; set; }

        public long headId { get; set; }

        public Decimal amount { get; set; }

        public string notes { get; set; }

        public List<ApiTransactionMetaPayload> apiTransactionMeta { get; set; }

        public string apiName { get; set; }
    }
}