using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models
{
    public class ApiResponse : IApiResponse
    {
        public string correlationId { get; set; }

        public ApiResponseMeta meta { get; set; }
    }
}