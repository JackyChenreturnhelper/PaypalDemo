using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models.test
{
    public class PaypalCaptureAmount
    {
        [JsonProperty(PropertyName = "currency_code")]
        public string currencyCode { get; set; }

        public Decimal value { get; set; }
    }
}