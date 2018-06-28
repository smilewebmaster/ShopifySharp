﻿using Newtonsoft.Json;
using System;

namespace ShopifySharp
{
    public class PriceRuleDiscountCode : ShopifyObject
    {        
        /// <summary>
        /// The discount code.
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("price_rule_id")]
        /// <summary>
        /// Unique numeric identifier for the price rule.
        /// </summary>
        public long? PriceRuleId { get; set; }

        [JsonProperty("usage_count")]
        /// <summary>
        /// Number of times the discount code has been redeemed.
        /// </summary>
        public int? UsageCount { get; set; }

        /// <summary>
        /// The date and time when the discount code was created. The API returns this value in ISO 8601 format.
        /// </summary>
        [JsonProperty("created_at", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// The date and time when the discount code was last modified. The API returns this value in ISO 8601 format.
        /// </summary>
        [JsonProperty("updated_at", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTimeOffset? UpdatedAt { get; set; }

    }
}
