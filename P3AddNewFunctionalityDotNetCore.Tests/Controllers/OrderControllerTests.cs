using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests.Controllers
{
    public class OrderControllerTests : DbContextTestBase
    {
        private readonly OrderController _orderController;
        private readonly OrderService _orderService;
        private readonly ProductService _productService;
        private readonly ICart _cart;

        public OrderControllerTests()
        {
            Mock<IStringLocalizer<OrderController>> stringLocalizerMock = new Mock<IStringLocalizer<OrderController>>();
            stringLocalizerMock.Setup(s => s[It.IsAny<string>()]).Returns(new LocalizedString(string.Empty, string.Empty));

            _cart = new Cart();

            _productService = new ProductService(_cart, new ProductRepository(DbContextInMemoryDb),
                new OrderRepository(DbContextInMemoryDb),
                new Mock<IStringLocalizer<ProductService>>().Object);
            SeedData.Initialize(DbContextOptionsInMemory);
            _orderService = new OrderService(_cart, new OrderRepository(new P3Referential(DbContextOptionsInMemory)), _productService);

            _orderController = new OrderController(_cart, 
            _orderService, 
            stringLocalizerMock.Object);
        }

        [Fact]
        public void IndexTest()
        {
            var result = _orderController.Index();

            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            Assert.IsAssignableFrom<OrderViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async void IndexSaveOrderTest()
        {
            _cart.AddItem(_productService.GetProductById(1), 2);
            var order = new OrderViewModel
            {
                Name = "Name",
                Address = "Address",
                City = "City",
                Country = "Country",
                Date = new DateTime(2019, 9, 25, 11, 52, 0),
                Zip = "Zip"
            };

            var result = _orderController.Index(order);
            var savedOrder = (await _orderService.GetOrders()).Last();

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Completed", redirectResult.ActionName);
            Assert.Single(savedOrder.OrderLine);
            Assert.Equal(1, savedOrder.OrderLine.First().ProductId);
            Assert.Equal(2, savedOrder.OrderLine.First().Quantity);
        }

        [Fact]
        public async void IndexSaveCartEmptyTest()
        {
            var order = new OrderViewModel
            {
                Name = "Name",
                Address = "Address",
                City = "City",
                Country = "Country",
                Date = new DateTime(2019, 9, 25, 11, 52, 0),
                Zip = "Zip"
            };

            var result = _orderController.Index(order);

            Assert.IsType<ViewResult>(result);
            Assert.Empty(await _orderService.GetOrders());
        }
    }
}
