using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using Xunit;
using System.Linq;

namespace P3AddNewFunctionalityDotNetCore.Tests.Repositories
{
    public class OrderRepositoryTests : DbContextTestBase
    {
        private readonly IOrderRepository _orderRepositoryInMemoryDb;
        private readonly IOrderRepository _orderRepositoryRealDb;

        public OrderRepositoryTests()
        {
            _orderRepositoryRealDb = new OrderRepository(DbContextRealDb);

            _orderRepositoryInMemoryDb = new OrderRepository(DbContextInMemoryDb);
            SeedData.Initialize(DbContextOptionsInMemory);           
        }

        [Fact]
        public async void SaveTest()
        {
            var order = new Order
            {
                Name = "Peter Doe",
                Address = "Address",
                City = "City",
                Country = "Country",
                Zip = "Zip",
                Date = new DateTime(2019, 9, 17, 21, 6, 0),
                OrderLine = new List<OrderLine>
                {
                    new OrderLine
                    {
                        ProductId = 2,
                        Quantity = 2
                    }
                }
            };

            _orderRepositoryInMemoryDb.Save(order);

            var savedOrder = (await _orderRepositoryInMemoryDb.GetOrders()).Last();
            Assert.Equal(order.Name, savedOrder.Name);
            Assert.Equal(order.Address, savedOrder.Address);
            Assert.Equal(order.City, savedOrder.City);
            Assert.Equal(order.Country, savedOrder.Country);
            Assert.Equal(order.Zip, savedOrder.Zip);
            Assert.Equal(order.Date, savedOrder.Date);
            Assert.Equal(order.OrderLine.First().ProductId, savedOrder.OrderLine.First().ProductId);
            Assert.Equal(order.OrderLine.First().Quantity, savedOrder.OrderLine.First().Quantity);
        }

        // TODO Test where ProductId doesn't exist 
        // TODO where Product Stock is zero

        [Fact]
        public async void SaveNullTest()
        {
            _orderRepositoryInMemoryDb.Save(null);

            Assert.Equal(0, (await _orderRepositoryInMemoryDb.GetOrders()).Count);
        }

        [Fact]
        public async void GetOrderTest()
        {
            var order = await _orderRepositoryRealDb.GetOrder(1);

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
            var order = await _orderRepositoryInMemoryDb.GetOrder(9);

            Assert.Null(order);
        }

        [Fact]
        public async void GetOrdersTest()
        {
            var orders = await _orderRepositoryRealDb.GetOrders();

            Assert.Single(orders);
            Assert.Single(orders.First().OrderLine);
        }

        [Fact]
        public async void GetEmptyOrderTest()
        { 
            Assert.Empty(await _orderRepositoryInMemoryDb.GetOrders());
        }
    }
}
