using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Models.test.DTO
{
    public class PaypalAccessTokenResponse
    {
        [JsonProperty(PropertyName = "access_token")]
        public string accessToken { get; set; }

        public string error { get; set; }

        [JsonProperty(PropertyName = "error_description")]
        public string errorDescription { get; set; }

        public string GetErrorMessage()
        {
            string suggestedAction = "Please contact Customer Support";
            string errorMessage = $"Unauthorized. {error}: {errorDescription}";
            return $"{errorMessage}. {suggestedAction}";
        }
    }
}