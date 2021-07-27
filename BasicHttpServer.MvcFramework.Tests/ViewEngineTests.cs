using System;
using System.Collections.Generic;
using System.IO;
using BasicHttpServer.MvcFramework.ViewEngine;
using Xunit;

namespace BasicHttpServer.MvcFramework.Tests
{
    public class ViewEngineTests
    {
        [Theory]
        [InlineData("CleanHtml")]
        [InlineData("Foreach")]
        [InlineData("IfElseFor")]
        [InlineData("ViewModel")]
        public void TestGetHtml(string fileName)
        {
            var viewModel = new TestViewModel
            {
                DateOfBirth = new DateTime(2019, 6, 1),
                Name = "Dogo Argentina",
                Price = 12345.67M
            };

            IViewEngine viewEngine = new ViewEngine.ViewEngine();
            var view = File.ReadAllText($"ViewTests/{fileName}.html");
            var result = viewEngine.GetHtml(view, viewModel);
            var expectedResult = File.ReadAllText($"ViewTests/{fileName}.Result.html");
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void TestTemplateViewModel()
        {
            IViewEngine viewEngine = new ViewEngine.ViewEngine();
            var actualResult = viewEngine.GetHtml(@"@foreach(var num in Model)
{
<span>@num</span>
}", new List<int> { 1, 2, 3 });

            var expectedResult = @"<span>1</span>
<span>2</span>
<span>3</span>
";

            Assert.Equal(expectedResult, actualResult);
        }
    }
}
