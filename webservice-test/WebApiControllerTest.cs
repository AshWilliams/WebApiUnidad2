using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiUnidad2.Controllers;
using Xunit;

namespace webservice_test
{
    public class WebApiControllerTest
    {
        ValuesController _controller;
        private static IConfiguration configuration;
        public WebApiControllerTest()
        {
            _controller = new ValuesController(configuration);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = _controller.Get();

            // Assert
            Assert.IsType<ActionResult<string>>(okResult.Result);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsAllItemsAsync()
        {
            // Act
            var okResult = _controller.Get();

            // Assert
            var items = Assert.IsType<ActionResult<string>>(okResult.Result);
            Assert.Equal("Oooops, Acceso No Autorizado!!!", ((Microsoft.AspNetCore.Mvc.ObjectResult)items.Result).Value);
        }
    }
}
