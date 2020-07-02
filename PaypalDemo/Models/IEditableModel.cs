using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models
{
    public interface IEditableModel : ICreateOnlyModel
    {
        DateTime GetModifyOn();

        string GetModifyBy();

        void SetModifyOn(DateTime modifyOn);

        void SetModifyBy(string modifyBy);
    }
}