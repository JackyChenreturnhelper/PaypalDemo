using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models
{
    public class ApiTransactionDefinition : CreateOnlyModel, IDbModel<long>
    {
        [Key]
        [Column("api_transaction_id")]
        public long apiTransactionId { get; set; }

        [Column("transaction_number")]
        public string transactionNumber { get; set; }

        [Column("api_id")]
        public int apiId { get; set; }

        [Column("currency_code")]
        public string currencyCode { get; set; }

        [Column("transaction_type")]
        public string transactionType { get; set; }

        [Column("head_id")]
        public long headId { get; set; }

        [Column("amount")]
        public decimal amount { get; set; }

        [Column("notes")]
        public string notes { get; set; }

        public long GetKey()
        {
            return apiTransactionId;
        }

        public void SetKey(long key)
        {
            apiTransactionId = key;
        }
    }
}