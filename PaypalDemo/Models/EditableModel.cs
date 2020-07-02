using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models
{
    public abstract class EditableModel : CreateOnlyModel, IEditableModel
    {
        [Column("modify_on")]
        public DateTime modifyOn { get; set; }

        [Column("modify_by")]
        public string modifyBy { get; set; }

        public virtual DateTime GetModifyOn()
        {
            return modifyOn;
        }

        public virtual string GetModifyBy()
        {
            return modifyBy;
        }

        public virtual void SetModifyOn(DateTime modifyOn)
        {
            this.modifyOn = modifyOn;
        }

        public void SetModifyBy(string modifyBy)
        {
            this.modifyBy = modifyBy;
        }
    }
}