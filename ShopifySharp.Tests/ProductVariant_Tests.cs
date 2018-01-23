using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ShopifySharp.Filters;
using Xunit;
using EmptyAssert = ShopifySharp.Tests.Extensions.EmptyExtensions;

namespace ShopifySharp.Tests
{
    [Trait("Category", "ProductVariant")]
    public class ProductVariant_Tests : IClassFixture<ProductVariant_Tests_Fixture>
    {
        private ProductVariant_Tests_Fixture Fixture { get; }

        public ProductVariant_Tests(ProductVariant_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public async Task Counts_Variants()
        {
            var count = await Fixture.Service.CountAsync(Fixture.ProductId);

            Assert.True(count > 0);
        }

        [Fact]
        public async Task Lists_Variants()
        {
            var list = await Fixture.Service.ListAsync(Fixture.ProductId);

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public async Task Deletes_Variants()
        {
            var created = await Fixture.Create(skipAddToCreatedList: true);
            bool threw = false;

            try
            {
                await Fixture.Service.DeleteAsync(Fixture.ProductId, created.Id.Value);
            }
            catch (ShopifyException ex)
            {
                Console.WriteLine($"{nameof(Deletes_Variants)} failed. {ex.Message}");

                threw = true;
            }

            Assert.False(threw);
        }

        [Fact]
        public async Task Gets_Variants()
        {
            var created = await Fixture.Service.GetAsync(Fixture.Created.First().Id.Value);

            Assert.NotNull(created);
            Assert.True(created.Id.HasValue);
            Assert.Equal(Fixture.Price, created.Price);
            EmptyAssert.NotNullOrEmpty(created.Option1);
        }

        [Fact]
        public async Task Creates_Variants()
        {
            var created = await Fixture.Create();

            Assert.NotNull(created);
            Assert.True(created.Id.HasValue);
            Assert.Equal(Fixture.Price, created.Price);
            EmptyAssert.NotNullOrEmpty(created.Option1);
        }

        [Fact]
        public async Task Updates_Variants()
        {
            decimal newPrice = (decimal) 11.22;
            var created = await Fixture.Create();
            long id = created.Id.Value;

            created.Price = newPrice;
            created.Id = null;

            var updated = await Fixture.Service.UpdateAsync(id, created);

            // Reset the id so the Fixture can properly delete this object.
            created.Id = id;

            Assert.Equal(newPrice, updated.Price);
        }
    }

    public class ProductVariant_Tests_Fixture : IAsyncLifetime
    {
        public ProductVariantService Service { get; } = new ProductVariantService(Utils.MyShopifyUrl, Utils.AccessToken);

        public List<ProductVariant> Created { get; } = new List<ProductVariant>();

        public decimal Price => 123.45m;

        public long ProductId { get; set; }

        public async Task InitializeAsync()
        {
            // Get a product id to use with these tests.
            ProductId = (await new ProductService(Utils.MyShopifyUrl, Utils.AccessToken).ListAsync(new ProductFilter()
            {
                Limit = 1
            })).First().Id.Value;

            // Create one for use with count, list, get, etc. tests.
            await Create();
        }

        public async Task DisposeAsync()
        {
            foreach (var obj in Created)
            {
                if (! obj.Id.HasValue) 
                {
                    continue; 
                }

                try
                {
                    await Service.DeleteAsync(ProductId, obj.Id.Value);
                }
                catch (ShopifyException ex)
                {
                    if (ex.HttpStatusCode != HttpStatusCode.NotFound)
                    {
                        Console.WriteLine($"Failed to delete created ProductVariant with id {obj.Id.Value}. {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public async Task<ProductVariant> Create(string option1 = null, bool skipAddToCreatedList = false)
        {
            var obj = await Service.CreateAsync(ProductId, new ProductVariant()
            {
                Option1 = Guid.NewGuid().ToString(),
                Price = Price,
            });

            if (! skipAddToCreatedList)
            {
                Created.Add(obj);
            }

            return obj;
        }
    }
}
