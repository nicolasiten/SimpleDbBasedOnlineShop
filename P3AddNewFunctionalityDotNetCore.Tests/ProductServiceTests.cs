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
using System.Linq;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceTests
    {
        private readonly IProductService _productService;
        private readonly ICart _cart;
        private readonly Mock<IStringLocalizer<ProductService>> _localizer;

        public ProductServiceTests()
        {
            var p3ReferentialOptions = new DbContextOptionsBuilder<P3Referential>().UseInMemoryDatabase("P3Database").Options;
            P3Referential p3Referential = new P3Referential(p3ReferentialOptions);

            _cart = new Cart();

            _localizer = new Mock<IStringLocalizer<ProductService>>();

            _productService = new ProductService(_cart, new ProductRepository(p3Referential), new OrderRepository(p3Referential), _localizer.Object);
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

        // TODO CheckModelErrors For all scenarios --> find out how to use localizer in tests

            // TODO Fix TestOrder
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
    }
}