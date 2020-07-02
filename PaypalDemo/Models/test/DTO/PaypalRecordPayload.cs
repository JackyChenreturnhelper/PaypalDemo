using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models.test.DTO
{
    public class PaypalRecordPayload : CreateOnlyPayload
    {
        public long paypalRecordId { get; set; }
        public int companyId { get; set; }
        public string currencyCode { get; set; }
        public Decimal amount { get; set; }
        public string apiTransactionNumber { get; set; }
        public long apiTransactionId { get; set; }
        public long apiFileId { get; set; }
        public string paypalOrderId { get; set; }
        public string paypalCaptureId { get; set; }
    }
}