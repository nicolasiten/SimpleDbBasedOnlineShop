using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using P3AddNewFunctionalityDotNetCore.Data;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests.Repositories
{
    public class ProductRepositoryTests : DbContextTestBase
    {
        private readonly IProductRepository _productRepositoryInMemoryDb;
        private readonly IProductRepository _productRepositoryRealDb;

        public ProductRepositoryTests()
        {
            _productRepositoryRealDb = new ProductRepository(DbContextRealDb);

            _productRepositoryInMemoryDb = new ProductRepository(DbContextInMemoryDb);
            SeedData.Initialize(DbContextOptionsInMemory);
        }

        [Theory]
        [InlineData(1, false, "Echo Dot", "(2nd Generation) - Black", 10, 92.5)]
        [InlineData(10000, true)]
        [InlineData(0, true)]
        [InlineData(-1, true)]
        public async void GetProductTest(int id, bool expectNull, string name = "", string description = "", int quantity = 0, double price = 0)
        {
            var product = await _productRepositoryRealDb.GetProduct(id);

            if (expectNull)
            {
                Assert.Null(product);
            }
            else
            {
                Assert.Equal(name, product.Name);
                Assert.Equal(description, product.Description);
                Assert.Equal(quantity, product.Quantity);
                Assert.Equal(price, product.Price);
            }
        }

        [Fact]
        public void GetAllProductsTest()
        {
            var products = _productRepositoryRealDb.GetAllProducts();

            Assert.IsType<List<Product>>(products);
            Assert.Equal(5, products.Count());
        }

        [Fact]
        public void GetAllProductsEmptyTest()
        {
            foreach (var product in _productRepositoryInMemoryDb.GetAllProducts())
            {
                _productRepositoryInMemoryDb.DeleteProduct(product.Id);
            }

            Assert.Empty(_productRepositoryInMemoryDb.GetAllProducts());
        }

        [Fact]
        public void UpdateProductStocksTest()
        {
            _productRepositoryInMemoryDb.UpdateProductStocks(1, 3);

            Assert.Equal(7, _productRepositoryInMemoryDb.GetProduct(1).Result.Quantity);
        }

        [Fact]
        public void UpdateNonExistingProductStocksTest()
        {
            _productRepositoryInMemoryDb.UpdateProductStocks(1111, 3);
        }

        [Fact]
        public void SaveProductTest()
        {
            var product = new Product
            {
                Name = "TestProduct",
                Description = "Description",
                Details = "Details",
                Quantity = 2,
                Price = 1,
            };

            _productRepositoryInMemoryDb.SaveProduct(product);

            var savedProduct = _productRepositoryInMemoryDb.GetAllProducts().Last();
            Assert.Equal(product.Name, savedProduct.Name);
            Assert.Equal(product.Description, savedProduct.Description);
            Assert.Equal(product.Details, savedProduct.Details);
            Assert.Equal(product.Quantity, savedProduct.Quantity);
            Assert.Equal(product.Price, savedProduct.Price);
        }

        [Fact]
        public void RemoveNonExistingProductTest()
        {
            _productRepositoryInMemoryDb.DeleteProduct(6);

            Assert.Equal(5, _productRepositoryInMemoryDb.GetAllProducts().Count());
        }

        [Fact]
        public void RemoveProductTest()
        {
            _productRepositoryInMemoryDb.DeleteProduct(1);

            Assert.Equal(4, _productRepositoryInMemoryDb.GetAllProducts().Count());
        }
    }
}
