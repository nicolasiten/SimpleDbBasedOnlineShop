using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class CartTests
    {
        private readonly ICart _cart;

        public CartTests()
        {
            _cart = new Cart();
            _cart.AddItem(new Product
            {
                Id = 1,
                Description = "Description",
                Details = "Details",
                Name = "Name",
                Price = 10,
                Quantity = 10
            }, 1);
            _cart.AddItem(new Product
            {
                Id = 2,
                Description = "Description",
                Details = "Details",
                Name = "Name",
                Price = 15,
                Quantity = 9
            }, 2);
        }

        [Fact]
        public void AddItemTest()
        {
            var product = new Product
            {
                Id = 3,
                Description = "Description",
                Details = "Details",
                Name = "Name",
                Price = 8,
                Quantity = 11
            };

            _cart.AddItem(product, 1);

            var cartLine = _cart.Lines.Last();

            Assert.Equal(product.Id, cartLine.Product.Id);
            Assert.Equal(product.Description, cartLine.Product.Description);
            Assert.Equal(product.Details, cartLine.Product.Details);
            Assert.Equal(product.Name, cartLine.Product.Name);
            Assert.Equal(product.Quantity, cartLine.Product.Quantity);
            Assert.Equal(product.Price, cartLine.Product.Price);
            Assert.Equal(1, cartLine.Quantity);
        }

        [Fact]
        public void AddExistingTest()
        {
            var product = new Product
            {
                Id = 1
            };

            _cart.AddItem(product, 2);

            var cartLine = _cart.Lines.First(p => p.Product.Id == 1);

            Assert.Equal(3, cartLine.Quantity);
        }

        [Fact]
        public void GetAverageValueTest()
        {
            double averageValue = _cart.GetAverageValue();
            double expectedValue = (15 * 2 + 10) / 3d;

            Assert.Equal(expectedValue, averageValue);
        }

        [Fact]
        public void GetTotalValueTest()
        {
            double totalValue = _cart.GetTotalValue();
            double expectedValue = 15 * 2 + 10;

            Assert.Equal(expectedValue, totalValue);
        }
    }
}
