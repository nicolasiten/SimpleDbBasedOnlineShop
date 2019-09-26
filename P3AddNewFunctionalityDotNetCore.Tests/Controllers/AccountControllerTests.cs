using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using P3AddNewFunctionalityDotNetCore.Controllers;
using Xunit;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace P3AddNewFunctionalityDotNetCore.Tests.Controllers
{
    public class AccountControllerTests
    {
        private readonly AccountController _accountController;
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<SignInManager<IdentityUser>> _signInManagerMock;

        public AccountControllerTests()
        {
            var userStoreMock = new Mock<IUserStore<IdentityUser>>();

            _userManagerMock = new Mock<UserManager<IdentityUser>>(userStoreMock.Object,
                null, null, null, null, null, null, null, null);

            var contextAccessorMock = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();

            _signInManagerMock = new Mock<SignInManager<IdentityUser>>(_userManagerMock.Object,
                contextAccessorMock.Object, userPrincipalFactory.Object, null, null, null);

            _accountController = new AccountController(_userManagerMock.Object, _signInManagerMock.Object);
        }

        [Fact]
        public async void LoginInvalidModelTest()
        {
            var loginModel = new LoginModel
            {
                Name = null,
                Password = "Pw1234",
                ReturnUrl = "url"
            };

            _accountController.ModelState.AddModelError(string.Empty, string.Empty);
            var result = await _accountController.Login(loginModel);

            Assert.IsAssignableFrom<ViewResult>(result);
            _userManagerMock.Verify(u => u.FindByNameAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async void LoginValidModelTest()
        {
            var loginModel = new LoginModel
            {
                Name = "Name",
                Password = "Pw1234",
                ReturnUrl = "url"
            };
            _userManagerMock
                .Setup(u => u.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(new IdentityUser(loginModel.Name)));
            _signInManagerMock
                .Setup(s => s.PasswordSignInAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<bool>(),
                    It.IsAny<bool>())).Returns(Task.FromResult(SignInResult.Success));

            var result = await _accountController.Login(loginModel);

            var redirectResult = Assert.IsAssignableFrom<RedirectResult>(result);
            Assert.Equal(loginModel.ReturnUrl, redirectResult.Url);
            _userManagerMock.Verify(u => u.FindByNameAsync(loginModel.Name), Times.Once);
            _signInManagerMock.Verify(u => u.SignOutAsync(), Times.Once);
            _signInManagerMock.Verify(u => 
                u.PasswordSignInAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public async void LogoutTest()
        {
            var result = await _accountController.Logout("url");

            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("url", redirectResult.Url);
            _signInManagerMock.Verify(s => s.SignOutAsync(), Times.Once);
        }
    }
}
