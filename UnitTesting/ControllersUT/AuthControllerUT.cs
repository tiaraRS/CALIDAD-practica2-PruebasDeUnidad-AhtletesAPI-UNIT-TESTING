using AthletesRestAPI.Controllers;
using AthletesRestAPI.Data.Entity;
using AthletesRestAPI.Data.Repository;
using AthletesRestAPI.Exceptionss;
using AthletesRestAPI.Models;
using AthletesRestAPI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AthletesRestAPI.Models.Security;
using AthletesRestAPI.Services.Security;

namespace UnitTesting.ControllersUT
{
    public class AuthControllerUT
    {
        //RegisterAsync
        //tc1
        [Fact]
        public async Task RegisterUser_ReturnsStatusCode200()
        {
            //ARRANGE
            var registerModel = new RegisterViewModel()
            {
                Email = "testmail@gmail.com",
                Password = "SecretPass1234",
                ConfirmPassword = "SecretPass1234"
            };
            var userResponseModel = new UserManagerResponse()
            {
                Token = "User created successfully!",
                IsSuccess = true,
            };
            var registerServiceMock = new Mock<IUserService>();
            registerServiceMock.Setup(r => r.RegisterUserAsync(registerModel)).ReturnsAsync(userResponseModel);

            //ACT
            var registerController = new AuthController(registerServiceMock.Object);
            var response = await registerController.RegisterAsync(registerModel);
            var status = response.Result as OkObjectResult;
            var userResponse = status.Value as UserManagerResponse;

            //ASSERT
            Assert.True(userResponse.IsSuccess);
        }

        //tc2
        [Fact]
        public async Task RegisterUser_ReturnsBadRequestDifferentPasswords()
        {
            //ARRANGE
            var registerModel = new RegisterViewModel()
            {
                Email = "testmail@gmail.com",
                Password = "SecretPass1234",
                ConfirmPassword = "AnotherPass1234"
            };
            var userResponseModel = new UserManagerResponse()
            {
                Token = "Confirm password doesn't match the password!",
                IsSuccess = false,
            };
            var registerServiceMock = new Mock<IUserService>();
            registerServiceMock.Setup(r => r.RegisterUserAsync(registerModel)).ReturnsAsync(userResponseModel);

            //ACT
            var registerController = new AuthController(registerServiceMock.Object);
            var response = await registerController.RegisterAsync(registerModel);
            var status = response.Result as BadRequestObjectResult;
            var userResponse = status.Value as UserManagerResponse;

            //ASSERT
            Assert.False(userResponse.IsSuccess);
        }

        //tc3
        [Fact]
        public async Task RegisterUser_ReturnsBadRequestInvalidModel()
        {
            //ARRANGE
            var registerModel = new RegisterViewModel()
            {
                Email = "nem",
                Password = "nope",
                ConfirmPassword = "n"
            };
            var userResponseModel = new UserManagerResponse()
            {
                Token = "User did not create",
                IsSuccess = false,
            };
            var registerServiceMock = new Mock<IUserService>();

            //ACT
            var registerController = new AuthController(registerServiceMock.Object);
            registerController.ModelState.AddModelError("test", "modelNotValid");
            var response = await registerController.RegisterAsync(registerModel);
            var actualStatusCode = ((BadRequestObjectResult)response.Result).StatusCode;

            //ASSERT
            Assert.Equal(400, actualStatusCode);
        }
    }
}

