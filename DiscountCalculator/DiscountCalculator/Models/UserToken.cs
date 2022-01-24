using System;
using Newtonsoft.Json;

namespace DiscountCalculator
{
    public class UserToken
    {
        [JsonProperty("userId")]
        public Guid UserId { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
