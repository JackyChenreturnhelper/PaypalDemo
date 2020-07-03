using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models
{
    public class PaymentRequest
    {
        public string intent { get; set; }
        public Payer payer { get; set; }
        public Transaction[] transactions { get; set; }
        public Redirect_Urls redirect_urls { get; set; }

        public class Payer
        {
            public string payment_method { get; set; }
            public Funding_Instruments[] funding_instruments { get; set; }
        }

        public class Funding_Instruments
        {
            public Billing billing { get; set; }
        }

        public class Billing
        {
            public string billing_agreement_id { get; set; }
        }

        public class Redirect_Urls
        {
            public string return_url { get; set; }
            public string cancel_url { get; set; }
        }

        public class Transaction
        {
            public Amount amount { get; set; }
            public string description { get; set; }
            public Item_List item_list { get; set; }
        }

        public class Amount
        {
            public string currency { get; set; }
            public string total { get; set; }
        }

        public class Item_List
        {
            public Item[] items { get; set; }
        }

        public class Item
        {
            public string sku { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string quantity { get; set; }
            public string price { get; set; }
            public string currency { get; set; }
            public string tax { get; set; }
            public string url { get; set; }
        }
    }
}