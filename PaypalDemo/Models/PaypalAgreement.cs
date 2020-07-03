using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models
{
    public class PaypalAgreement
    {
        public List<LinkAgreement> links { get; set; }
        public string token_id { get; set; }
    }

    public class LinkAgreement
    {
        public string href { get; set; }
        public string rel { get; set; }
        public string method { get; set; }
    }
}