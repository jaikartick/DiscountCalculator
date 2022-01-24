using System;
using Newtonsoft.Json;

namespace DiscountCalculator.DomainModel.Models
{
    public class EntityBase
    {
        [JsonProperty("createdOn")]
        public DateTimeOffset CreatedOn { get; set; }

        [JsonProperty("createdBy")]
        public Guid CreatedBy { get; set; }

        [JsonProperty("lastModifiedOn")]
        public DateTimeOffset LastModifiedOn { get; set; }

        [JsonProperty("lastModifiedBy")]
        public Guid LastModifiedBy { get; set; }

        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
