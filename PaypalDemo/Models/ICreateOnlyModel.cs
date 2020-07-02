using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models
{
    public interface ICreateOnlyModel
    {
        DateTime GetCreateOn();

        string GetCreateBy();

        void SetCreateOn(DateTime createOn);

        void SetCreateBy(string createBy);
    }
}