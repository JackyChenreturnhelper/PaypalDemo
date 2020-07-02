using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models
{
    public interface IApiResponse
    {
        string correlationId { get; set; }

        ApiResponseMeta meta { get; set; }
    }
}