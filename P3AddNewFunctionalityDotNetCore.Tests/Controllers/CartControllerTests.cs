using Microsoft.AspNetCore.Mvc;
using Moq;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests.Controllers
{
    public class CartControllerTests
    {
        private readonly CartController _cartController;
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<ICart> _cartMock;

        public CartControllerTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _cartMock = new Mock<ICart>();
            _cartController = new CartController(_cartMock.Object, _productServiceMock.Object);
        }

        [Fact]
        public void IndexTest()
        {
            var result = _cartController.Index();

            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Fact]
        public void AddToCartTest()
        {
            var product = new Product
            {
                Id = 1,
                Description = "Description",
                Details = "Details",
                Name = "Name",
                Price = 1,
                Quantity = 2
            };
            _productServiceMock.Setup(p => p.GetProductById(It.IsAny<int>())).Returns(product);
            _cartMock.Setup(c => c.AddItem(It.IsAny<Product>(), It.IsAny<int>()));

            var result = _cartController.AddToCart(1);

            _cartMock.Verify(c => c.AddItem(product, 1));
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Null(redirectResult.ControllerName);
        }

        [Fact]
        public void AddToCartNonExistingTest()
        {
            _productServiceMock.Setup(p => p.GetProductById(It.IsAny<int>())).Returns((Product)null);
            _cartMock.Setup(c => c.AddItem(It.IsAny<Product>(), It.IsAny<int>()));

            var result = _cartController.AddToCart(1);

            _cartMock.Verify(c => c.AddItem(It.IsAny<Product>(), 1), Times.Never);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Product", redirectResult.ControllerName);
        }

        [Fact]
        public void RemoveFromCartTest()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Id = 1
                },
                new Product
                {
                    Id = 2
                }
            };
            _productServiceMock.Setup(p => p.GetAllProducts()).Returns(products);
            _cartMock.Setup(c => c.RemoveLine(It.IsAny<Product>()));

            var result = _cartController.RemoveFromCart(1);

            _cartMock.Verify(c => c.RemoveLine(products.First()));
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public void RemoveFromCartNonExistingTest()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Id = 1
                },
                new Product
                {
                    Id = 2
                }
            };
            _productServiceMock.Setup(p => p.GetAllProducts()).Returns(products);
            _cartMock.Setup(c => c.RemoveLine(It.IsAny<Product>()));

            var result = _cartController.RemoveFromCart(3);

            _cartMock.Verify(c => c.RemoveLine(It.IsAny<Product>()), Times.Never);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }
    }
}
