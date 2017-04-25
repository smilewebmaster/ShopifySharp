using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ShopifySharp.Tests
{
    [Trait("Category", "Transaction")]
    public class Transaction_Tests : IClassFixture<Transaction_Tests_Fixture>
    {
        private Transaction_Tests_Fixture Fixture { get; }

        public Transaction_Tests(Transaction_Tests_Fixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public async Task Counts_Transactions()
        {
            var count = await Fixture.Service.CountAsync(Fixture.Created.First().OrderId);

            Assert.True(count > 0);
        }

        [Fact]
        public async Task Lists_Transactions()
        {
            var list = await Fixture.Service.ListAsync(Fixture.Created.First().OrderId);

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public async Task Gets_Transactions()
        {
            var obj = await Fixture.Service.GetAsync(Fixture.Created.First().OrderId, Fixture.Created.First().Id.Value);

            Assert.NotNull(obj);
            Assert.True(obj.Id.HasValue);
            Assert.Null(obj.ErrorCode);
            Assert.Equal(Fixture.Amount, obj.Amount);
            Assert.Equal(Fixture.Currency, obj.Currency);
            Assert.Equal(Fixture.Status, obj.Status);
            Assert.Equal(Fixture.Gateway, obj.Gateway);
        }

        [Fact]
        public async Task Creates_Transactions()
        {
            var order = await Fixture.CreateOrder();
            var obj = await Fixture.Create(order.Id.Value);

            Assert.NotNull(obj);
            Assert.True(obj.Id.HasValue);
            Assert.Null(obj.ErrorCode);
            Assert.Equal(Fixture.Amount, obj.Amount);
            Assert.Equal(Fixture.Currency, obj.Currency);
            Assert.Equal(Fixture.Status, obj.Status);
            Assert.Equal(Fixture.Gateway, obj.Gateway);
        }

        [Fact]
        public async Task Creates_Capture_Transactions()
        {
            string kind = "capture";
            var order = await Fixture.CreateOrder();
            var obj = await Fixture.Create(order.Id.Value, kind);

            Assert.Equal("success", obj.Status);
            Assert.Equal(kind, obj.Kind);
            Assert.Null(obj.ErrorCode);
        }

        [Fact(Skip = "This test returns the error 'Order cannot be refunded'. Orders that were created via API, not using a Shopify transaction gateway, cannot be refunded. Therefore, refunds are untestable.")]
        public async Task Creates_Refund_Transactions()
        {
            string kind = "refund";
            var order = await Fixture.CreateOrder();
            var obj = await Fixture.Create(order.Id.Value, kind);

            Assert.Equal("success", obj.Status);
            Assert.Equal(kind, obj.Kind);
            Assert.Null(obj.ErrorCode);
        }

        [Fact]
        public async Task Creates_A_Void_Transaction()
        {
            string kind = "void";
            var order = await Fixture.CreateOrder();
            var obj = await Fixture.Create(order.Id.Value, kind);

            Assert.Equal("success", obj.Status);
            Assert.Equal(kind, obj.Kind);
            Assert.Null(obj.ErrorCode);
        }
    }

    public class Transaction_Tests_Fixture : IAsyncLifetime
    {
        public TransactionService Service => new TransactionService(Utils.MyShopifyUrl, Utils.AccessToken);

        public OrderService OrderService => new OrderService(Utils.MyShopifyUrl, Utils.AccessToken);

        public List<Transaction> Created { get; } = new List<Transaction>();

        public List<Order> CreatedOrders { get; } = new List<Order>();

        public double Amount => 10.00;

        public string Currency => "USD";

        public string Gateway => "bogus";

        public string Status => "success";

        public long OrderId { get; set; }

        public async Task InitializeAsync()
        {
            // Create one collection for use with count, list, get, etc. tests.
            var order = await CreateOrder();
            await Create(order.Id.Value);
        }

        public async Task DisposeAsync()
        {
            foreach (var obj in CreatedOrders)
            {
                try
                {
                    await OrderService.DeleteAsync(obj.Id.Value);
                }
                catch (ShopifyException ex)
                {
                    if (ex.HttpStatusCode != HttpStatusCode.NotFound)
                    {
                        Console.WriteLine($"Failed to delete created Order with id {obj.Id.Value}. {ex.Message}");
                    }
                }
            }
        }

        public async Task<Order> CreateOrder()
        {
            var obj = await OrderService.CreateAsync(new Order()
            {

            }, new OrderCreateOptions()
            {
                SendFulfillmentReceipt = false,
                SendReceipt = false
            });

            CreatedOrders.Add(obj);

            return obj;
        }

        /// <summary>
        /// Convenience function for running tests. Creates an object and automatically adds it to the queue for deleting after tests finish.
        /// </summary>
        public async Task<Transaction> Create(long orderId, string kind = "authorization", bool skipAddToCreatedList = false)
        {
            var obj = await Service.CreateAsync(orderId, new Transaction()
            {
                Amount = Amount,
                Currency = Currency,
                Gateway = Gateway,
                Status = Status,
                Test = true,
                Kind = kind
            });

            if (! skipAddToCreatedList)
            {
                Created.Add(obj);
            }

            return obj;
        }
    }
}