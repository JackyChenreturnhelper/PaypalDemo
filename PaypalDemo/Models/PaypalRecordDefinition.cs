using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models
{
    public class PaypalRecordDefinition : CreateOnlyModel, IDbModel<long>
    {
        [Key]
        [Column("paypal_record_id")]
        public long paypalRecordId { get; set; }

        [Column("company_id")]
        public int companyId { get; set; }

        [Column("paypal_payment_id")]
        public string paypalPaymentId { get; set; }

        [Column("currency_code")]
        public string currencyCode { get; set; }

        [Column("amount")]
        public decimal amount { get; set; }

        [Column("api_transaction_number")]
        public string apiTransactionNumber { get; set; }

        [Column("api_transaction_id")]
        public long apiTransactionId { get; set; }

        [Column("api_file_id")]
        public long apiFileId { get; set; }

        [Column("paypal_order_id")]
        public string paypalOrderId { get; set; }

        [Column("paypal_capture_id")]
        public string paypalCaptureId { get; set; }

        public long GetKey()
        {
            return paypalRecordId;
        }

        public void SetKey(long key)
        {
            paypalRecordId = key;
        }
    }
}