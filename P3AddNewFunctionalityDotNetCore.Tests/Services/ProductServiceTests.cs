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
    public class ProductServiceTests
    {
        private readonly IProductService _productService;
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
            var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
            var p3ReferentialOptions = new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            P3Referential p3Referential = new P3Referential(p3ReferentialOptions);

            _cart = new Cart();

            _localizerMock = new Mock<IStringLocalizer<ProductService>>();
            _localizerMock.Setup(l => l["MissingName"]).Returns(new LocalizedString("MissingName", _missingName));
            _localizerMock.Setup(l => l["MissingPrice"]).Returns(new LocalizedString("MissingPrice", _missingPrice));
            _localizerMock.Setup(l => l["PriceNotANumber"]).Returns(new LocalizedString("PriceNotANumber", _priceNotANumber));
            _localizerMock.Setup(l => l["PriceNotGreaterThanZero"]).Returns(new LocalizedString("PriceNotGreaterThanZero", _priceNotGreaterThanZero));
            _localizerMock.Setup(l => l["MissingStock"]).Returns(new LocalizedString("MissingStock", _missingStock));
            _localizerMock.Setup(l => l["StockNotAnInteger"]).Returns(new LocalizedString("StockNotAnInteger", _stockNotAnInteger));
            _localizerMock.Setup(l => l["StockNotGreaterThanZero"]).Returns(new LocalizedString("StockNotGreaterThanZero", _stockNotGreaterThanZero));

            _productService = new ProductService(_cart, new ProductRepository(p3Referential), new OrderRepository(p3Referential), _localizerMock.Object);
            SeedData.Initialize(p3ReferentialOptions);
        }

        [Fact]
        public void GetAllProductsViewModelTest()
        {
            var products = _productService.GetAllProductsViewModel();

            Assert.IsType<List<ProductViewModel>>(products);
            Assert.Equal(5, products.Count);
        }

        [Fact]
        public void GetAllProductsTest()
        {
            var products = _productService.GetAllProducts();

            Assert.IsType<List<Product>>(products);
            Assert.Equal(5, products.Count);
        }

        [Fact]
        public void GetProductViewModelByInvalidIdTest()
        {
            var product = _productService.GetProductByIdViewModel(-1);

            Assert.Null(product);
        }

        [Fact]
        public void GetProductViewModelByIdTest()
        {
            var product = _productService.GetProductByIdViewModel(1);

            Assert.Equal("Echo Dot", product.Name);
            Assert.Equal("(2nd Generation) - Black", product.Description);
            Assert.Equal("10", product.Stock);
            Assert.Equal("92.5", product.Price);
        }

        [Fact]
        public void GetProductByIdTest()
        {
            var product = _productService.GetProductById(2);

            Assert.Equal("Anker 3ft / 0.9m Nylon Braided", product.Name);
            Assert.Equal("Tangle-Free Micro USB Cable", product.Description);
            Assert.Equal(20, product.Quantity);
            Assert.Equal(9.99, product.Price);
        }

        [Fact]
        public void UpdateProductQuantitesTest()
        {
            _cart.AddItem(_productService.GetProductById(1), 1);

            _productService.UpdateProductQuantities();

            Assert.Equal(9, _productService.GetProductById(1).Quantity);
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

            List<string> errors = _productService.CheckProductModelErrors(product);

            Assert.Equal(string.Join(",", messages), string.Join(",", errors));
        }

        [Fact]
        public void SaveEmptyProductTest()
        {
            var emptyProduct = new ProductViewModel();

            Assert.Throws<ArgumentNullException>(() => _productService.SaveProduct(emptyProduct));
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

            _productService.SaveProduct(product);

            var lastProduct = _productService.GetAllProducts().Last();

            Assert.Equal(product.Name, lastProduct.Name);
            Assert.Equal(product.Description, lastProduct.Description);
            Assert.Equal(product.Details, lastProduct.Details);
            Assert.Equal(2, lastProduct.Price);
            Assert.Equal(3, lastProduct.Quantity);
        }

        [Fact]
        public void RemoveProductTest()
        {
            _productService.DeleteProduct(1);

            Assert.Equal(4, _productService.GetAllProducts().Count);
        }
    }
}