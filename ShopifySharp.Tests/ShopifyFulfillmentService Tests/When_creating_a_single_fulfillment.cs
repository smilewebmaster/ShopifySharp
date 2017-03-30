﻿using Machine.Specifications;
using ShopifySharp.Tests.Test_Data;
using System.Linq;

namespace ShopifySharp.Tests.ShopifyFulfillmentService_Tests
{
    [Subject(typeof(FulfillmentService))]
    class When_creating_a_single_fulfillment
    {
        Establish context = () =>
        {
            Service = new FulfillmentService(Utils.MyShopifyUrl, Utils.AccessToken);
            Order = FulfillmentCreation.CreateOrder().Await().AsTask.Result;
        };

        Because of = () =>
        {
            var fulfillment = FulfillmentCreation.GenerateFulfillment(false, Order.LineItems.Take(1));
            
            Fulfillment = Service
                .CreateAsync(Order.Id.Value, fulfillment, false)
                .Await()
                .AsTask
                .Result;
        };

        It should_create_a_single_fulfillment = () =>
        {
            Fulfillment.ShouldNotBeNull();
            Fulfillment.Id.HasValue.ShouldBeTrue();
            Fulfillment.Status.Equals("success", System.StringComparison.OrdinalIgnoreCase).ShouldBeTrue();
        };

        Cleanup after = () =>
        {
            FulfillmentCreation.DeleteOrder(Order.Id.Value).Await();
        };

        static FulfillmentService Service;

        static ShopifyFulfillment Fulfillment;

        static Order Order;
    }
}
