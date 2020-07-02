using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models
{
    public interface IDeleteable
    {
        void SetIsDeleted(bool isDeleted);
    }
}