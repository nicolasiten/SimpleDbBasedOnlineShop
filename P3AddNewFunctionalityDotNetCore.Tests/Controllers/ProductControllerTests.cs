using Microsoft.AspNetCore.Mvc;
using Moq;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Localization;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests.Controllers
{
    public class ProductControllerTests : DbContextTestBase
    {
        private readonly ProductController _productControllerRealDb;
        private readonly ProductController _productControllerInMemoryDb;
        private readonly IProductService _productServiceInMemoryDb;

        public ProductControllerTests()
        {
            Mock<IStringLocalizer<ProductService>> stringLocalizerMock = new Mock<IStringLocalizer<ProductService>>();
            stringLocalizerMock.Setup(l => l[It.IsAny<string>()]).Returns(new LocalizedString(string.Empty, string.Empty));

            IProductService productServiceRealDb = new ProductService(new Cart(), 
                new ProductRepository(new P3Referential(DbContextOptionsRealDb)), 
                new OrderRepository(new P3Referential(DbContextOptionsRealDb)), 
                stringLocalizerMock.Object);
            _productControllerRealDb = new ProductController(productServiceRealDb, new LanguageService());

            _productServiceInMemoryDb = new ProductService(new Cart(),
                new ProductRepository(new P3Referential(DbContextOptionsInMemory)),
                new OrderRepository(new P3Referential(DbContextOptionsInMemory)),
                stringLocalizerMock.Object);
            SeedData.Initialize(DbContextOptionsInMemory);
            _productControllerInMemoryDb = new ProductController(_productServiceInMemoryDb, new LanguageService());
        }

        [Fact]
        public void IndexTest()
        {
            var result = _productControllerRealDb.Index();

            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(5, model.Count());
        }

        [Fact]
        public void AdminTest()
        {
            var result = _productControllerRealDb.Admin();

            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(5, model.Count());
            Assert.Equal("Echo Dot", model.First(m => m.Id == 1).Name);
        }

        [Fact]
        public void CreateModelStateValidTest()
        {
            var product = new ProductViewModel
            {
                Description = "Description",
                Details = "Details",
                Name = "Name",
                Price = "1",
                Stock = "2"
            };

            var result = _productControllerInMemoryDb.Create(product);
            var savedProduct = _productServiceInMemoryDb.GetAllProductsViewModel().Last();

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Admin", redirectResult.ActionName);
            Assert.Equal(product.Description, savedProduct.Description);
            Assert.Equal(product.Details, savedProduct.Details);
            Assert.Equal(product.Name, savedProduct.Name);
            Assert.Equal(product.Price, savedProduct.Price);
            Assert.Equal(product.Stock, savedProduct.Stock);
        }

        [Fact]
        public void CreateModelStateInValidTest()
        {
            var product = new ProductViewModel
            {
                Id = 1,
                Description = "Description",
                Details = "Details",
                Name = "Name",
                Price = "-1",
                Stock = "2"
            };

            var result = _productControllerInMemoryDb.Create(product);

            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductViewModel>(viewResult.ViewData.Model);
            Assert.Equal(5, _productServiceInMemoryDb.GetAllProducts().Count);
        }

        [Fact]
        public void DeleteProductTest()
        {
            var result = _productControllerInMemoryDb.DeleteProduct(2);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Admin", redirectResult.ActionName);
            Assert.Equal(4, _productServiceInMemoryDb.GetAllProducts().Count);
        }
    }
}
