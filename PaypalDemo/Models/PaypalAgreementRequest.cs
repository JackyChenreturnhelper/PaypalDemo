using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models
{
    public class PaypalAgreementRequest
    {
        public string description { get; set; }
        public Shipping_Address shipping_address { get; set; }
        public Payer payer { get; set; }
        public Plan plan { get; set; }

        public class Shipping_Address
        {
            public string line1 { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string postal_code { get; set; }
            public string country_code { get; set; }
            public string recipient_name { get; set; }
        }

        public class Payer
        {
            public string payment_method { get; set; }
        }

        public class Plan
        {
            public string type { get; set; }
            public Merchant_Preferences merchant_preferences { get; set; }
        }

        public class Merchant_Preferences
        {
            public string return_url { get; set; }
            public string cancel_url { get; set; }
            public string accepted_pymt_type { get; set; }
            public bool skip_shipping_address { get; set; }
            public bool immutable_shipping_address { get; set; }
        }
    }
}