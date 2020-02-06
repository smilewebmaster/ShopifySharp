﻿using ShopifySharp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ShopifySharp.Filters;

namespace ShopifySharp
{
    /// <summary>
    /// A service for adjusting existing Shopify gift cards.
    /// </summary>
    public class GiftCardAdjustmentService : ShopifyService
    {
        /// <summary>
        /// Creates a new instance of <see cref="GiftCardAdjustmentService" />.
        /// </summary>
        /// <param name="myShopifyUrl">The shop's *.myshopify.com URL.</param>
        /// <param name="shopAccessToken">An API access token for the shop.</param>
        public GiftCardAdjustmentService(string myShopifyUrl, string shopAccessToken) : base(myShopifyUrl, shopAccessToken)
        {
        }

        /// <summary>
        /// Gets a list of gift card adjustments belonging to the given gift card.
        /// </summary>
        /// <param name="giftCardId">The gift card that the adjustment was applied to.</param>
        public virtual async Task<IEnumerable<GiftCardAdjustment>> ListAsync(IListFilter filter)
        {
            throw new Exception("not yet implemented");
            // var req = PrepareRequest($"gift_cards/{giftCardId}/adjustments.json");
            //
            // return await ExecuteRequestAsync<List<GiftCardAdjustment>>(req, HttpMethod.Get, rootElement: "adjustments");
        }

        /// <summary>
        /// Retrieves the <see cref="GiftCardAdjustment"/> with the given id.
        /// </summary>
        /// <param name="giftCardId">The gift card that the adjustment was applied to.</param>
        /// <param name="adjustmentId">The id of the adjustment to retrieve.</param>
        /// <returns></returns>
        public virtual async Task<GiftCardAdjustment> GetAsync(long giftCardId, long adjustmentId)
        {
            var req = PrepareRequest($"gift_cards/{giftCardId}/adjustments/{adjustmentId}.json");
            var response = await ExecuteRequestAsync<GiftCardAdjustment>(req, HttpMethod.Get, rootElement: "adjustment");
            return response.Result;
        }

        /// <summary>
        /// Apply an adjustment to an existing gift card <see cref="GiftCardAdjustment"/>
        /// </summary>
        /// <param name="giftCardId">The gift card that the adjustment was applied to.</param>
        /// <param name="adjustment">A new <see cref="GiftCardAdjustment"/>. Signed amount and note should be the only properties set.</param>
        /// <returns></returns>
        public virtual async Task<GiftCardAdjustment> CreateAsync(long giftCardId, GiftCardAdjustment adjustment)
        {
            var req = PrepareRequest($"gift_cards/{giftCardId}/adjustments.json");
            var content = new JsonContent(new
            {
                adjustment = adjustment
            });

            var response = await ExecuteRequestAsync<GiftCardAdjustment>(req, HttpMethod.Post, content, "adjustment");
            return response.Result;
        }
    }
}
