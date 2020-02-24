using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShopifySharp.Filters
{
    /// <summary>
    /// Options for filtering lists of Themes. 
    /// </summary>
    public class ThemeListFilter : IUnpaginatedListFilter
    {
        /// <summary>
        /// Retrieve only certain fields, specified by a comma-separated list of field names. 
        /// </summary>
        [JsonProperty("fields")]
        public string Fields { get; set; }
        
        public IEnumerable<KeyValuePair<string, object>> ToQueryParameters()
        {
            throw new NotImplementedException();
        }
    }
}
