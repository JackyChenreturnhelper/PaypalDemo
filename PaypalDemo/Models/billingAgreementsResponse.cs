using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models
{
    public class billingAgreementsResponse
    {
        public string id { get; set; }
        public string state { get; set; }
        public string description { get; set; }
        public Payer payer { get; set; }
        public Plan plan { get; set; }
        public Link[] links { get; set; }
        public Merchant merchant { get; set; }
        public Shipping_Address shipping_address { get; set; }
        public DateTime create_time { get; set; }
        public DateTime update_time { get; set; }

        public class Payer
        {
            public Payer_Info payer_info { get; set; }
        }

        public class Payer_Info
        {
            public string email { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string payer_id { get; set; }
        }

        public class Plan
        {
            public string type { get; set; }
            public Merchant_Preferences merchant_preferences { get; set; }
        }

        public class Merchant_Preferences
        {
            public string notify_url { get; set; }
            public string accepted_pymt_type { get; set; }
        }

        public class Merchant
        {
            public Payee_Info payee_info { get; set; }
        }

        public class Payee_Info
        {
            public string email { get; set; }
        }

        public class Shipping_Address
        {
            public string recipient_name { get; set; }
            public string line1 { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string postal_code { get; set; }
            public string country_code { get; set; }
        }

        public class Link
        {
            public string href { get; set; }
            public string rel { get; set; }
            public string method { get; set; }
        }
    }
}