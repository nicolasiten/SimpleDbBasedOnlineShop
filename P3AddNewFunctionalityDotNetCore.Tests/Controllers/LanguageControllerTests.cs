using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using P3AddNewFunctionalityDotNetCore.Controllers;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests.Controllers
{
    public class LanguageControllerTests
    {
        private readonly LanguageController _languageController;
        private readonly Mock<ILanguageService> _languageServiceMock;

        public LanguageControllerTests()
        {
            _languageServiceMock = new Mock<ILanguageService>();
            _languageController = new LanguageController(_languageServiceMock.Object);
        }

        [Theory]
        [InlineData("English")]
        [InlineData("French")]
        [InlineData("")]
        public void ChangeUiLanguageTest(string language)
        {
            var languageModel = new LanguageViewModel
            {
                Language = language == string.Empty ? null : language
            };
            _languageServiceMock.Setup(l => l.ChangeUiLanguage(It.IsAny<HttpContext>(), It.IsAny<string>()));

            var result = _languageController.ChangeUiLanguage(languageModel, "Url");

            if (language != string.Empty)
            {
                _languageServiceMock.Verify(l => l.ChangeUiLanguage(It.IsAny<HttpContext>(), language));
            }
            else
            {
                _languageServiceMock.Verify(l => l.ChangeUiLanguage(It.IsAny<HttpContext>(), language), Times.Never);
            }
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("Url", redirectResult.Url);
        }
    }
}
