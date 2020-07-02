using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models
{
    public abstract class CreateOnlyModel : ICreateOnlyModel
    {
        [Column("create_on")]
        public DateTime createOn { get; set; }

        [Column("create_by")]
        public string createBy { get; set; }

        public virtual DateTime GetCreateOn()
        {
            return createOn;
        }

        public virtual string GetCreateBy()
        {
            return createBy;
        }

        public virtual void SetCreateOn(DateTime createOn)
        {
            this.createOn = createOn;
        }

        public virtual void SetCreateBy(string createBy)
        {
            this.createBy = createBy;
        }
    }
}