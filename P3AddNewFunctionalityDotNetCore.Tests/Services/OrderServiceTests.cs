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
    public class OrderServiceTests
    {
        private readonly IOrderService _orderService;

        public OrderServiceTests()
        {
            var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
            var p3ReferentialOptions = new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            IOrderRepository orderRepository = new OrderRepository(new P3Referential(p3ReferentialOptions));

            // seed data
            SeedData.Initialize(p3ReferentialOptions);
            using (var context = new P3Referential(p3ReferentialOptions))
            {
                context.Order.AddRange(
                    new Order
                    {
                        Name = "John Doe",
                        Address = "Address",
                        City = "City",
                        Country = "Country",
                        Zip = "Zip",
                        Date = new DateTime(2019, 9, 17, 21, 6, 0),
                        OrderLine = new List<OrderLine>
                        {
                            new OrderLine
                            {
                                ProductId = 1,
                                Quantity = 1
                            }
                        }
                    });

                context.SaveChanges();
            }

            _orderService = new OrderService(new Cart(), 
                orderRepository,
                new Mock<IProductService>().Object);
        }

        [Fact]
        public async void GetOrderTest()
        {
            var order = await _orderService.GetOrder(1);

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
            var order = await _orderService.GetOrder(9);

            Assert.Null(order);
        }

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
                            Id = 1,
                            Quantity = 1
                        }
                    }
                }
            };

            _orderService.SaveOrder(order);

            Assert.Equal(2, (await _orderService.GetOrders()).Count);
            var savedOrder = (await _orderService.GetOrders()).Last();
            Assert.Equal(order.Name, savedOrder.Name);
            Assert.Equal(order.Address, savedOrder.Address);
            Assert.Equal(order.City, savedOrder.City);
            Assert.Equal(order.Country, savedOrder.Country);
            Assert.Equal(order.Date, new DateTime(2019, 9, 18, 20, 41, 0));
            Assert.Equal(order.Lines.First().Product.Id, savedOrder.OrderLine.First().ProductId);
        }
    }
}
