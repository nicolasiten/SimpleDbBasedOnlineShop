using P3AddNewFunctionalityDotNetCore.Models.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests.Services
{
    public class LanguageServiceTests
    {
        private readonly ILanguageService _languageService;

        public LanguageServiceTests()
        {
            _languageService = new LanguageService();
        }

        [Theory]
        [InlineData("English", "en")]
        [InlineData("French", "fr")]
        [InlineData("Spanish", "en")]
        [InlineData("", "en")]
        public void SetCultureTest(string language, string expectedResult)
        {
            string result = _languageService.SetCulture(language);

            Assert.Equal(expectedResult, result);
        }
    }
}
