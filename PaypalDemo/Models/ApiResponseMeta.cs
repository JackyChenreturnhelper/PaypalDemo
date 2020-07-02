using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PaypalDemo.Models
{
    public class ApiResponseMeta
    {
        public HttpStatusCode status { get; set; }

        public Dictionary<string, string> data { get; set; }

        public string errorCode { get; set; }

        public Dictionary<string, string> error { get; set; }

        public ApiResponseMeta()
        {
            this.data = new Dictionary<string, string>();
            this.error = new Dictionary<string, string>();
        }
    }
}