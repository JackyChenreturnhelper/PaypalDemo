using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models
{
    public class PaypalOrderParamterModel
    {
        public string billingToken { get; set; }

        [Key]
        public string orderID { get; set; }

        public string facilitatorAccessToken { get; set; }

        public string payerID { get; set; }

        public DateTime? create_on { get; set; } = DateTime.UtcNow;
    }
}