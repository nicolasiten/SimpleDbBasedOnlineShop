using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.Collections.Generic;
using Xunit;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace P3AddNewFunctionalityDotNetCore.Tests.Services
{
    public class ProductServiceTests : DbContextTestBase
    {
        private readonly IProductService _productServiceInMemoryDb;
        private readonly IProductService _productServiceRealDb;
        private readonly ICart _cart;
        private readonly Mock<IStringLocalizer<ProductService>> _localizerMock;

        private const string _missingName = "Please enter a name";
        private const string _missingPrice = "Please enter a price value";
        private const string _priceNotANumber = "The value entered for the price must be a number";
        private const string _priceNotGreaterThanZero = "The price must be greater than zero";
        private const string _missingStock = "Please enter a stock value";
        private const string _stockNotAnInteger = "The value entered for the stock must be a integer";
        private const string _stockNotGreaterThanZero = "The stock must be greater than zero";

        public ProductServiceTests()
        {
            _localizerMock = new Mock<IStringLocalizer<ProductService>>();
            _localizerMock.Setup(l => l["MissingName"]).Returns(new LocalizedString("MissingName", _missingName));
            _localizerMock.Setup(l => l["MissingPrice"]).Returns(new LocalizedString("MissingPrice", _missingPrice));
            _localizerMock.Setup(l => l["PriceNotANumber"]).Returns(new LocalizedString("PriceNotANumber", _priceNotANumber));
            _localizerMock.Setup(l => l["PriceNotGreaterThanZero"]).Returns(new LocalizedString("PriceNotGreaterThanZero", _priceNotGreaterThanZero));
            _localizerMock.Setup(l => l["MissingStock"]).Returns(new LocalizedString("MissingStock", _missingStock));
            _localizerMock.Setup(l => l["StockNotAnInteger"]).Returns(new LocalizedString("StockNotAnInteger", _stockNotAnInteger));
            _localizerMock.Setup(l => l["StockNotGreaterThanZero"]).Returns(new LocalizedString("StockNotGreaterThanZero", _stockNotGreaterThanZero));

            _cart = new Cart();

            _productServiceInMemoryDb = new ProductService(
                _cart, 
                new ProductRepository(new P3Referential(DbContextOptionsInMemory)), 
                new OrderRepository(new P3Referential(DbContextOptionsInMemory)), 
                _localizerMock.Object);
            SeedData.Initialize(DbContextOptionsInMemory);

            _productServiceRealDb = new ProductService(
                _cart,
                new ProductRepository(new P3Referential(DbContextOptionsRealDb)),
                new OrderRepository(new P3Referential(DbContextOptionsRealDb)),
                _localizerMock.Object);
        }

        [Fact]
        public void GetAllProductsViewModelTest()
        {
            var products = _productServiceRealDb.GetAllProductsViewModel();

            Assert.IsType<List<ProductViewModel>>(products);
            Assert.Equal(5, products.Count);
        }

        [Fact]
        public void GetAllProductsTest()
        {
            var products = _productServiceRealDb.GetAllProducts();

            Assert.IsType<List<Product>>(products);
            Assert.Equal(5, products.Count);
        }

        [Fact]
        public void GetProductViewModelByInvalidIdTest()
        {
            var product = _productServiceRealDb.GetProductByIdViewModel(-1);

            Assert.Null(product);
        }

        [Fact]
        public void GetProductViewModelByIdTest()
        {
            var product = _productServiceRealDb.GetProductByIdViewModel(1);

            Assert.Equal("Echo Dot", product.Name);
            Assert.Equal("(2nd Generation) - Black", product.Description);
            Assert.Equal("10", product.Stock);
            Assert.Equal("92.5", product.Price);
        }

        [Fact]
        public void GetProductByIdTest()
        {
            var product = _productServiceRealDb.GetProductById(2);

            Assert.Equal("Anker 3ft / 0.9m Nylon Braided", product.Name);
            Assert.Equal("Tangle-Free Micro USB Cable", product.Description);
            Assert.Equal(20, product.Quantity);
            Assert.Equal(9.99, product.Price);
        }

        [Fact]
        public void UpdateProductQuantitesTest()
        {
            _cart.AddItem(_productServiceInMemoryDb.GetProductById(1), 1);

            _productServiceInMemoryDb.UpdateProductQuantities();

            Assert.Equal(9, _productServiceInMemoryDb.GetProductById(1).Quantity);
        }

        [Fact]
        public void UpdateProductQuantitiesTooBigTest()
        {
            _cart.AddItem(_productServiceInMemoryDb.GetProductById(1), 11);

            Assert.Throws<ArgumentException>(() => _productServiceInMemoryDb.UpdateProductQuantities());
        }

        [Theory]
        [InlineData("name", "1", "1")]
        [InlineData("", "1", "1", _missingName)] 
        [InlineData("name", "", "1", _missingPrice, _priceNotANumber)] 
        [InlineData("name", "-1", "1", _priceNotGreaterThanZero)] 
        [InlineData("name", "1", "", _missingStock, _stockNotAnInteger)] 
        [InlineData("name", "1", "-1", _stockNotGreaterThanZero)] 
        public void CheckProductModelErrorsTest(string name, string price, string stock, params string[] messages)
        {
            var product = new ProductViewModel
            {
                Name = name,
                Price = price,
                Stock = stock
            };

            List<string> errors = _productServiceInMemoryDb.CheckProductModelErrors(product);

            Assert.Equal(string.Join(",", messages), string.Join(",", errors));
        }

        [Fact]
        public void SaveEmptyProductTest()
        {
            var emptyProduct = new ProductViewModel();

            Assert.Throws<ArgumentNullException>(() => _productServiceInMemoryDb.SaveProduct(emptyProduct));
        }

        [Fact]
        public void SaveProductTest()
        {
            var product = new ProductViewModel
            {
                Name = "TestProduct",
                Description = "Description",
                Details = "Details",
                Price = "2",
                Stock = "3"
            };

            _productServiceInMemoryDb.SaveProduct(product);

            var lastProduct = _productServiceInMemoryDb.GetAllProducts().Last();

            Assert.Equal(product.Name, lastProduct.Name);
            Assert.Equal(product.Description, lastProduct.Description);
            Assert.Equal(product.Details, lastProduct.Details);
            Assert.Equal(2, lastProduct.Price);
            Assert.Equal(3, lastProduct.Quantity);
        }

        [Fact]
        public void RemoveProductTest()
        {
            _productServiceInMemoryDb.DeleteProduct(1);

            Assert.Equal(4, _productServiceInMemoryDb.GetAllProducts().Count);
        }

        [Fact]
        public void RemoveProductAddedToCartTest()
        {
            _cart.AddItem(_productServiceInMemoryDb.GetProductById(1), 1);

            _productServiceInMemoryDb.DeleteProduct(1);

            Assert.Empty(_cart.Lines);
        }
    }
}