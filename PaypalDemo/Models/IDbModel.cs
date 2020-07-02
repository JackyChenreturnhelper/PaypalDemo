using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models
{
    public interface IDbModel<TKeyType>
    {
        TKeyType GetKey();

        void SetKey(TKeyType key);
    }
}