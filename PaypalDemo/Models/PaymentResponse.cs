using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models
{
    public class PaymentResponse
    {
        public string id { get; set; }
        public string intent { get; set; }
        public string state { get; set; }
        public Payer payer { get; set; }
        public Transaction[] transactions { get; set; }
        public Redirect_Urls redirect_urls { get; set; }
        public DateTime create_time { get; set; }
        public DateTime update_time { get; set; }
        public Link1[] links { get; set; }

        public class Payer
        {
            public string payment_method { get; set; }
            public string status { get; set; }
            public Payer_Info payer_info { get; set; }
            public Funding_Instruments[] funding_instruments { get; set; }
        }

        public class Payer_Info
        {
            public string email { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string payer_id { get; set; }
            public string country_code { get; set; }
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
            public Payee payee { get; set; }
            public string description { get; set; }
            public Item_List item_list { get; set; }
            public Related_Resources[] related_resources { get; set; }
        }

        public class Amount
        {
            public string total { get; set; }
            public string currency { get; set; }
            public Details details { get; set; }
        }

        public class Details
        {
            public string subtotal { get; set; }
            public string shipping { get; set; }
            public string insurance { get; set; }
            public string handling_fee { get; set; }
            public string shipping_discount { get; set; }
        }

        public class Payee
        {
            public string merchant_id { get; set; }
            public string email { get; set; }
        }

        public class Item_List
        {
            public Item[] items { get; set; }
        }

        public class Item
        {
            public string name { get; set; }
            public string sku { get; set; }
            public string description { get; set; }
            public string price { get; set; }
            public string currency { get; set; }
            public string tax { get; set; }
            public int quantity { get; set; }
        }

        public class Related_Resources
        {
            public Sale sale { get; set; }
        }

        public class Sale
        {
            public string id { get; set; }
            public string state { get; set; }
            public Amount1 amount { get; set; }
            public string payment_mode { get; set; }
            public string protection_eligibility { get; set; }
            public Transaction_Fee transaction_fee { get; set; }
            public string billing_agreement_id { get; set; }
            public string parent_payment { get; set; }
            public DateTime create_time { get; set; }
            public DateTime update_time { get; set; }
            public Link[] links { get; set; }
        }

        public class Amount1
        {
            public string total { get; set; }
            public string currency { get; set; }
            public Details1 details { get; set; }
        }

        public class Details1
        {
            public string subtotal { get; set; }
            public string shipping { get; set; }
            public string insurance { get; set; }
            public string handling_fee { get; set; }
            public string shipping_discount { get; set; }
        }

        public class Transaction_Fee
        {
            public string value { get; set; }
            public string currency { get; set; }
        }

        public class Link
        {
            public string href { get; set; }
            public string rel { get; set; }
            public string method { get; set; }
        }

        public class Link1
        {
            public string href { get; set; }
            public string rel { get; set; }
            public string method { get; set; }
        }
    }
}