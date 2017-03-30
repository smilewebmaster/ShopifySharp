﻿using Machine.Specifications;
using ShopifySharp.Tests.Test_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifySharp.Tests
{
    [Subject(typeof(OrderService))]
    public class When_opening_an_order
    {
        Establish context = () =>
        {
            Service = new OrderService(Utils.MyShopifyUrl, Utils.AccessToken);
            Id = Service.CreateAsync(OrderCreation.GenerateOrder()).Await().AsTask.Result.Id.Value;
            Service.CloseAsync(Id).Await();
        };

        Because of = () =>
        {
            Order = Service.OpenAsync(Id).Await().AsTask.Result;
        };

        It should_open_an_order = () =>
        {
            Order.ShouldNotBeNull();
            Order.Id.ShouldEqual(Id);
            Order.ClosedAt.HasValue.ShouldBeFalse();
        };

        Cleanup after = () =>
        {
            Service.DeleteAsync(Id).Await();
        };

        static OrderService Service;

        static Order Order;

        static long Id;
    }
}
