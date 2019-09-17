using Microsoft.AspNetCore.Mvc;
using Moq;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests.Controllers
{
    public class ProductControllerTests
    {
        private readonly ProductController _productController;
        private readonly Mock<IProductService> _productServiceMock;

        public ProductControllerTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _productServiceMock.Setup(p => p.GetAllProductsViewModel()).Returns(new List<ProductViewModel>
            {
                new ProductViewModel
                {
                    Id = 1,
                    Name = "A",
                    Description = "Description",
                    Price = "1",
                    Stock = "3"
                },
                new ProductViewModel
                {
                    Id = 2,
                    Name = "B",
                    Description = "Description",
                    Details = "Details",
                    Price = "2",
                    Stock = "4"
                }
            });

            _productController = new ProductController(_productServiceMock.Object, new LanguageService());
        }

        [Fact]
        public void IndexTest()
        {
            var result = _productController.Index();

            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public void AdminTest()
        {
            var result = _productController.Admin();

            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
            Assert.Equal("B", model.First().Name);
        }

        [Fact]
        public void CreateModelStateValidTest()
        {
            _productServiceMock.Setup(p => p.CheckProductModelErrors(It.IsAny<ProductViewModel>())).Returns(new List<string>());
            _productServiceMock.Setup(p => p.SaveProduct(It.IsAny<ProductViewModel>()));
            var product = new ProductViewModel
            {
                Id = 1,
                Description = "Description",
                Details = "Details",
                Name = "Name",
                Price = "1",
                Stock = "2"
            };

            var result = _productController.Create(product);

            _productServiceMock.Verify(p => p.CheckProductModelErrors(product));
            _productServiceMock.Verify(p => p.SaveProduct(product));
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Admin", redirectResult.ActionName);
        }

        [Fact]
        public void CreateModelStateInValidTest()
        {
            _productServiceMock.Setup(p => p.CheckProductModelErrors(It.IsAny<ProductViewModel>())).Returns(new List<string> { "The price must be greater than zero" });
            var product = new ProductViewModel
            {
                Id = 1,
                Description = "Description",
                Details = "Details",
                Name = "Name",
                Price = "-1",
                Stock = "2"
            };

            var result = _productController.Create(product);

            _productServiceMock.Verify(p => p.CheckProductModelErrors(product));
            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public void DeleteProductTest()
        {
            _productServiceMock.Setup(p => p.DeleteProduct(It.IsAny<int>()));

            var result = _productController.DeleteProduct(2);

            _productServiceMock.Verify(p => p.DeleteProduct(2));
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Admin", redirectResult.ActionName);
        }
    }
}
