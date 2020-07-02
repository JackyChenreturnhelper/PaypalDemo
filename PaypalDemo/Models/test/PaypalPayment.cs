using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models.test
{
    public class PaypalPayment
    {
        [JsonProperty(PropertyName = "captures")]
        public List<PaypalCapture> paypalCaptureList { get; set; }
    }
}