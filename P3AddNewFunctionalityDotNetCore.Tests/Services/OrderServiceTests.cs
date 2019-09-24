using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests.Services
{
    public class OrderServiceTests : DbContextTestBase
    {
        private readonly IOrderService _orderServiceInMemoryDb;
        private readonly IOrderService _orderServiceRealDb;

        public OrderServiceTests()
        {
            _orderServiceRealDb = new OrderService(new Cart(),
                new OrderRepository(new P3Referential(DbContextOptionsRealDb)),
                new Mock<IProductService>().Object);

            _orderServiceInMemoryDb = new OrderService(new Cart(),
                new OrderRepository(new P3Referential(DbContextOptionsInMemory)),
                new Mock<IProductService>().Object);
            SeedData.Initialize(DbContextOptionsInMemory);
        }

        [Fact]
        public async void GetOrderTest()
        {
            var order = await _orderServiceRealDb.GetOrder(1);

            Assert.Equal("John Doe", order.Name);
            Assert.Equal("Address", order.Address);
            Assert.Equal("City", order.City);
            Assert.Equal("Country", order.Country);
            Assert.Equal("Zip", order.Zip);
            Assert.Equal(new DateTime(2019, 9, 17, 21, 6, 0), order.Date);
            Assert.Equal(1, order.OrderLine.First().ProductId);
            Assert.Equal(1, order.OrderLine.First().Quantity);
        }

        [Fact]
        public async void GetNonExistingOrderTest()
        {
            var order = await _orderServiceRealDb.GetOrder(9);

            Assert.Null(order);
        }

        // TODO Test where ProductId doesn't exist 
        // TODO where Product Stock is zero

        [Fact]
        public async void SaveOrderTest()
        {
            var order = new OrderViewModel
            {
                Name = "Name",
                Address = "Address",
                City = "City",
                Country = "Country",
                Date = new DateTime(2019, 9, 18, 20, 41, 0),
                Lines = new List<CartLine>
                {
                    new CartLine
                    {
                        Product = new Product
                        {
                            Id = 1
                        },
                        Quantity = 1
                    }
                }
            };

            _orderServiceInMemoryDb.SaveOrder(order);

            Assert.Single(await _orderServiceInMemoryDb.GetOrders());
            var savedOrder = (await _orderServiceInMemoryDb.GetOrders()).Last();
            Assert.Equal(order.Name, savedOrder.Name);
            Assert.Equal(order.Address, savedOrder.Address);
            Assert.Equal(order.City, savedOrder.City);
            Assert.Equal(order.Country, savedOrder.Country);
            Assert.Equal(order.Date, new DateTime(2019, 9, 18, 20, 41, 0));
            Assert.Equal(order.Lines.First().Product.Id, savedOrder.OrderLine.First().ProductId);
        }

        [Fact]
        public async void GetOrdersTest()
        {
            var orders = await _orderServiceRealDb.GetOrders();

            Assert.Single(orders);
        }

        [Fact]
        public async void GetOrdersEmptyTest()
        {
            var orders = await _orderServiceInMemoryDb.GetOrders();

            Assert.Empty(orders);
        }
    }
}
