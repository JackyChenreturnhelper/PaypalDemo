using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models
{
    public class PaypalCapturePaymentResponseDemo
    {
        public string id { get; set; }
        public Purchase_Units[] purchase_units { get; set; }
        public Payer payer { get; set; }
        public Link1[] links { get; set; }
        public string status { get; set; }
    }

    public class Payer
    {
        public Name name { get; set; }
        public string email_address { get; set; }
        public string payer_id { get; set; }
        public Address address { get; set; }
    }

    public class Name
    {
        public string given_name { get; set; }
        public string surname { get; set; }
    }

    public class Address
    {
        public string country_code { get; set; }
    }

    public class Purchase_Units
    {
        public string reference_id { get; set; }
        public Shipping shipping { get; set; }
        public Payments payments { get; set; }
    }

    public class Shipping
    {
        public Name1 name { get; set; }
        public Address1 address { get; set; }
    }

    public class Name1
    {
        public string full_name { get; set; }
    }

    public class Address1
    {
        public string address_line_1 { get; set; }
        public string admin_area_2 { get; set; }
        public string admin_area_1 { get; set; }
        public string postal_code { get; set; }
        public string country_code { get; set; }
    }

    public class Payments
    {
        public Capture[] captures { get; set; }
    }

    public class Capture
    {
        public string id { get; set; }
        public string status { get; set; }
        public Amount amount { get; set; }
        public bool final_capture { get; set; }
        public Seller_Protection seller_protection { get; set; }
        public Seller_Receivable_Breakdown seller_receivable_breakdown { get; set; }
        public Link[] links { get; set; }
        public DateTime create_time { get; set; }
        public DateTime update_time { get; set; }
    }

    public class Amount
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }

    public class Seller_Protection
    {
        public string status { get; set; }
        public string[] dispute_categories { get; set; }
    }

    public class Seller_Receivable_Breakdown
    {
        public Gross_Amount gross_amount { get; set; }
        public Paypal_Fee paypal_fee { get; set; }
        public Net_Amount net_amount { get; set; }
    }

    public class Gross_Amount
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }

    public class Paypal_Fee
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }

    public class Net_Amount
    {
        public string currency_code { get; set; }
        public string value { get; set; }
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