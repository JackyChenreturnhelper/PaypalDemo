using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models
{
    public class ApiTransactionMetaPayload
    {
        public long apiTransactionMetaId { get; set; }

        public long apiTransactionId { get; set; }

        public string metaType { get; set; }

        public long headId { get; set; }

        public string metaText { get; set; }
    }
}