using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models
{
    public class PaypalTokenId
    {
        /// <summary>
        /// 對應不到註解
        /// </summary>
        public string token_id { get; set; }

        /// <summary>
        /// 對應不到註解
        /// </summary>
        public string payer_id { get; set; }

        [Key]
        /// <summary>
        /// 對應不到註解
        /// </summary>
        public string Email { get; set; }
    }
}