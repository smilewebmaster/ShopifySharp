﻿using System;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using ShopifySharp.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShopifySharp.Infrastructure;
using ShopifySharp.Lists;

namespace ShopifySharp
{
    /// <summary>
    /// A service for manipulating mapping between custom collections and collections
    /// </summary>
    public class CustomCollectionService : ShopifyService
    {
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public CustomCollectionService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken) { }

        /// <summary>
        /// Gets a list of up to 250 custom collections.
        /// </summary>
        public virtual async Task<IListResult<CustomCollection>> ListAsync(IListFilter filter)
        {
            var req = PrepareRequest("custom_collections.json");
            
            if (filter != null)
            {
                req.QueryParams.AddRange(filter.ToQueryParameters());
            }
            
            var response = await ExecuteRequestAsync<List<CustomCollection>>(req, HttpMethod.Get, rootElement: "custom_collections");

            return ParseLinkHeaderToListResult(response);
        }

        /// <summary>
        /// Gets a list of up to 250 custom collections.
        /// </summary>
        public virtual async Task<IListResult<CustomCollection>> ListAsync(CustomCollectionListFilter filter)
        {
            return await ListAsync((IListFilter) filter);
        }

        /// <summary>
        /// Creates a new <see cref="CustomCollection"/> Custom Collection
        /// </summary>
        /// <param name="customCollection">A new <see cref="CustomCollection"/>. Id should be set to null.</param>
        /// <returns>The new <see cref="CustomCollection"/>.</returns>
        public virtual async Task<CustomCollection> CreateAsync(CustomCollection customCollection)
        {
            var req = PrepareRequest("custom_collections.json");
            var content = new JsonContent(new
            {
                custom_collection = customCollection
            });

            var response = await ExecuteRequestAsync<CustomCollection>(req, HttpMethod.Post, content, "custom_collection");
            
            return response.Result;
        }

        private async Task<int> _CountAsync(ICountFilter options = null)
        {
            var req = PrepareRequest("custom_collections/count.json");
            
            if (options != null)
            {
                req.QueryParams.AddRange(options.ToQueryParameters());
            }
            
            var response = await ExecuteRequestAsync<int>(req, HttpMethod.Get, rootElement: "count");

            return response.Result;
        }
        
        /// <summary>
        /// Gets a count of all of the custom collections
        /// </summary>
        public virtual async Task<int> CountAsync(ICountFilter filter = null)
        {
            return await _CountAsync(filter);
        }

        /// <summary>
        /// Gets a count of all of the custom collections
        /// </summary>
        public virtual async Task<int> CountAsync(CustomCollectionCountFilter filter = null)
        {
            return await _CountAsync(filter);
        }
        
        /// <summary>
        /// Retrieves the <see cref="CustomCollection"/> with the given id.
        /// </summary>
        /// <param name="customCollectionId">The id of the custom collection to retrieve.</param>
        /// <param name="fields">A comma-separated list of fields to return.</param>
        /// <returns>The <see cref="CustomCollection"/>.</returns>
        public virtual async Task<CustomCollection> GetAsync(long customCollectionId, string fields = null)
        {
            var req = PrepareRequest($"custom_collections/{customCollectionId}.json");

            if (!string.IsNullOrEmpty(fields))
            {
                req.QueryParams.Add("fields", fields);
            }

            var result = await ExecuteRequestAsync<CustomCollection>(req, HttpMethod.Get, rootElement: "custom_collection");
            return result.Result;
        }

        /// <summary>
        /// Deletes a custom collection with the given Id.
        /// </summary>
        /// <param name="customCollectionId">The custom collection's Id.</param>
        public virtual async Task DeleteAsync(long customCollectionId)
        {
            var req = PrepareRequest($"custom_collections/{customCollectionId}.json");

            await ExecuteRequestAsync(req, HttpMethod.Delete);
        }

        /// <summary>
        /// Updates the given <see cref="CustomCollection"/>.
        /// </summary>
        /// <param name="customCollectionId">Id of the object being updated.</param>
        /// <param name="customCollection">The <see cref="CustomCollection"/> to update.</param>
        /// <returns>The updated <see cref="CustomCollection"/>.</returns>
        public virtual async Task<CustomCollection> UpdateAsync(long customCollectionId, CustomCollection customCollection)
        {
            var req = PrepareRequest($"custom_collections/{customCollectionId}.json");
            var content = new JsonContent(new
            {
                custom_collection = customCollection
            });

            var response = await ExecuteRequestAsync<CustomCollection>(req, HttpMethod.Put, content, "custom_collection");
            return response.Result;
        }
    }
}
