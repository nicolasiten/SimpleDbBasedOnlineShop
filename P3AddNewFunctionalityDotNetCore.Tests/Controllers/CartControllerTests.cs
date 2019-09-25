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
using Microsoft.Extensions.Localization;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests.Controllers
{
    public class CartControllerTests : DbContextTestBase
    {
        private readonly CartController _cartController;
        private readonly IProductService _productService;
        private readonly ICart _cart;

        public CartControllerTests()
        {
            Mock<IStringLocalizer<ProductService>> stringLocalizerMock = new Mock<IStringLocalizer<ProductService>>();
            _cart = new Cart();

            _productService = new ProductService(new Cart(),
                new ProductRepository(new P3Referential(DbContextOptionsRealDb)),
                new OrderRepository(new P3Referential(DbContextOptionsRealDb)),
                stringLocalizerMock.Object);
            _cartController = new CartController(_cart, _productService);
        }

        [Fact]
        public void IndexTest()
        {
            var product = _productService.GetProductById(1);
            _cart.AddItem(product, 2);
            var result = _cartController.Index();

            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Cart>(viewResult.Model);
            Assert.Single(model.Lines);
            Assert.Equal(product.Id, model.Lines.First().Product.Id);
            Assert.Equal(product.Description, model.Lines.First().Product.Description);
            Assert.Equal(product.Details, model.Lines.First().Product.Details);
            Assert.Equal(product.Name, model.Lines.First().Product.Name);
            Assert.Equal(product.Price, model.Lines.First().Product.Price);
            Assert.Equal(product.Quantity, model.Lines.First().Product.Quantity);
            Assert.Equal(2, model.Lines.First().Quantity);
        }

        [Fact]
        public void AddToCartTest()
        {
            var product = _productService.GetProductById(1);

            var result = _cartController.AddToCart(1);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Single(_cart.Lines);
            Assert.Equal(product.Id, _cart.Lines.First().Product.Id);
            Assert.Equal(product.Description, _cart.Lines.First().Product.Description);
            Assert.Equal(product.Details, _cart.Lines.First().Product.Details);
            Assert.Equal(product.Name, _cart.Lines.First().Product.Name);
            Assert.Equal(product.Price, _cart.Lines.First().Product.Price);
            Assert.Equal(product.Quantity, _cart.Lines.First().Product.Quantity);
            Assert.Equal(1, _cart.Lines.First().Quantity);
        }

        [Fact]
        public void AddToCartNonExistingTest()
        {
            var result = _cartController.AddToCart(8);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Product", redirectResult.ControllerName);
            Assert.Empty(_cart.Lines);
        }

        [Fact]
        public void RemoveFromCartTest()
        {
            _cartController.AddToCart(1);
            _cartController.AddToCart(2);

            var result = _cartController.RemoveFromCart(2);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Single(_cart.Lines);
            Assert.Equal(1, _cart.Lines.First().Product.Id);
        }

        [Fact]
        public void RemoveFromCartNonExistingTest()
        {
            _cartController.AddToCart(1);
            _cartController.AddToCart(2);

            var result = _cartController.RemoveFromCart(3);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal(2, _cart.Lines.Count());
        }
    }
}
