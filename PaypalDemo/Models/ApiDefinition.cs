using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models
{
    [Table("Api")]
    public class ApiDefinition : EditableModel, IDbModel<int>, IDeleteable
    {
        [Key]
        [Column("api_id")]
        public int apiId { get; set; }

        [Column("api_name")]
        public string apiName { get; set; }

        [Column("api_type_code")]
        public string apiTypeCode { get; set; }

        [Column("api_key")]
        public Guid apiKey { get; set; }

        [Column("api_secret")]
        public Guid apiSecret { get; set; }

        [Column("api_token")]
        public string apiToken { get; set; }

        [Column("primary_email")]
        public string primaryEmail { get; set; }

        [Column("is_deleted")]
        public bool isDeleted { get; set; }

        [Column("charge_factor")]
        public decimal chargeFactor { get; set; }

        public int GetKey()
        {
            return apiId;
        }

        public void SetKey(int key)
        {
            apiId = key;
        }

        public void SetIsDeleted(bool isDeleted)
        {
            this.isDeleted = isDeleted;
        }
    }
}