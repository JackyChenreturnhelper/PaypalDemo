using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models.test.DTO
{
    public class PaypalRecordDto
    {
        public PaypalRecordPayload paypalRecordPayload { get; set; }

        public PaypalAccessTokenResponse paypalAccessTokenResponse { get; set; }
        public PaypalCapturePaymentResponse paypalCapturePaymentResponse { get; set; }
        public string resultMessage { get; set; }
    }
}