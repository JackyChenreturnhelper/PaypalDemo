using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models.test.DTO
{
    public class PaypalCapturePaymentResponse
    {
        [JsonProperty(PropertyName = "id")]
        public string orderId { get; set; }

        [JsonProperty(PropertyName = "purchase_units")]
        public List<PaypalPurchaseUnit> paypalPurchaseUnitList { get; set; }

        public string name { get; set; }
        public string message { get; set; }

        public string GetErrorMessage()
        {
            string suggestedAction = "Payment is not processed. Please try again";
            string errorMessage = $"Order {orderId} capture payment failed: {name}: {message}";
            return $"{errorMessage}. {suggestedAction}";
        }
    }
}