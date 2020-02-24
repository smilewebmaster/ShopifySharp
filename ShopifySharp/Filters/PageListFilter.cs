﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShopifySharp.Filters
{
    /// <summary>
    /// Options for filtering lists of Pages. 
    /// </summary>
    public class PageListFilter : ListFilter
    {
        /// <summary>
        /// Filter by page title.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Filter by page handle.
        /// </summary>
        [JsonProperty("handle")]
        public string Handle { get; set; }
        
        /// <summary>
        /// Restrict results to after the specified ID.
        /// </summary>
        public long? SinceId { get; set; }
        
        /// <summary>
        /// Show those created at or after date.
        /// </summary>
        [JsonProperty("created_at_min")]
        public DateTimeOffset? CreatedAtMin { get; set; }

        /// <summary>
        /// Show those created at or after date.
        /// </summary>
        [JsonProperty("created_at_max")]
        public DateTimeOffset? CreatedAtMax { get; set; }
        
        /// <summary>
        /// Show those updated at or before date.
        /// </summary>
        [JsonProperty("updated_at_min")]
        public DateTimeOffset? UpdatedAtMin { get; set; }

        /// <summary>
        /// Show those last updated at or before date.
        /// </summary>
        [JsonProperty("updated_at_max")]
        public DateTimeOffset? UpdatedAtMax { get; set; }
        
        /// <summary>
        /// Show those published at or before date.
        /// </summary>
        [JsonProperty("published_at_min")]
        public DateTimeOffset? PublishedAtMin { get; set; }

        /// <summary>
        /// Show those last published at or before date.
        /// </summary>
        [JsonProperty("published_at_max")]
        public DateTimeOffset? PublishedAtMax { get; set; }
        
        /// <summary>
        /// Retrieve only certain fields, specified by a comma-separated list of field names. 
        /// </summary>
        [JsonProperty("fields")]
        public string Fields { get; set; }
        
        /// <summary>
        /// Restrict results to pages with a given published status. Known values: published, unpublished, any. Default: any. 
        /// </summary>
        [JsonProperty("status")]
        public string PublishedStatus { get; set; }

        public override IEnumerable<KeyValuePair<string, object>> ToQueryParameters()
        {
            throw new NotImplementedException();
        }
    }
}
