using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models.test
{
    public class PaypalCapture
    {
        [JsonProperty(PropertyName = "id")]
        public string captureId { get; set; }

        public PaypalCaptureAmount amount { get; set; }
    }
}