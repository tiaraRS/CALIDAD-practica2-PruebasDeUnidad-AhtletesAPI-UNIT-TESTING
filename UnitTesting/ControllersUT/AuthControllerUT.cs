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
using Newtonsoft.Json.Linq;

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
            var registerServiceMock = new Mock<IUserService>();

            //ACT
            var registerController = new AuthController(registerServiceMock.Object);
            registerController.ModelState.AddModelError("test", "modelNotValid");
            var response = await registerController.RegisterAsync(registerModel);
            var actualStatusCode = ((BadRequestObjectResult)response.Result).StatusCode;

            //ASSERT
            Assert.Equal(400, actualStatusCode);
        }

        //CreatRoleAsync
        //tc1
        [Fact]
        public async Task CreateRole_ReturnsStatusCode200()
        {
            //ARRANGE
            var roleModel = new CreateRoleViewModel()
            {
                Name = "Admin",
                NormalizedName = "ADMIN",
            };
            var userResponseModel = new UserManagerResponse()
            {
                Token = "Role created successfully!",
                IsSuccess = true,
            };
            var serviceMock = new Mock<IUserService>();
            serviceMock.Setup(r => r.CreateRoleAsync(roleModel)).ReturnsAsync(userResponseModel);

            //ACT
            var authController = new AuthController(serviceMock.Object);
            var response = await authController.CreateRolenAsync(roleModel);
            var status = response.Result as OkObjectResult;
            var userResponse = status.Value as UserManagerResponse;

            //ASSERT
            Assert.True(userResponse.IsSuccess);
        }

        //tc2
        [Fact]
        public async Task CreateRole_ReturnsBadRequestResultNotSuccess()
        {
            //ARRANGE
            var roleModel = new CreateRoleViewModel()
            {
                Name = "NoRole",
                NormalizedName = "notNorMal",
            };
            var userResponseModel = new UserManagerResponse()
            {
                Token = "Role did not create",
                IsSuccess = false,
            };
            var serviceMock = new Mock<IUserService>();
            serviceMock.Setup(r => r.CreateRoleAsync(roleModel)).ReturnsAsync(userResponseModel);

            //ACT
            var authController = new AuthController(serviceMock.Object);
            var response = await authController.CreateRolenAsync(roleModel);
            var status = response.Result as BadRequestObjectResult;
            var userResponse = status.Value as UserManagerResponse;

            //ASSERT
            Assert.False(userResponse.IsSuccess);
        }

        //tc3
        [Fact]
        public async Task CreateRole_ReturnsBadRequestInvalidModel()
        {
            //ARRANGE
            var roleModel = new CreateRoleViewModel()
            {
                Name = null,
                NormalizedName = null,
            };
            var serviceMock = new Mock<IUserService>();

            //ACT
            var authController = new AuthController(serviceMock.Object);
            authController.ModelState.AddModelError("test", "modelNotValid");
            var response = await authController.CreateRolenAsync(roleModel);
            var actualStatusCode = ((BadRequestObjectResult)response.Result).StatusCode;

            //ASSERT
            Assert.Equal(400, actualStatusCode);
        }

        //CreatUserRoleAsync
        //tc1
        [Fact]
        public async Task CreateUserRole_ReturnsStatusCode200()
        {
            //ARRANGE
            var roleModel = new CreateUserRoleViewModel()
            {
                UserId = "a391ce2932cd-19ca392f32c2",
                RoleId = "ff3c465ocei1-35963lsto3is",
            };
            var userResponseModel = new UserManagerResponse()
            {
                Token = "Role assigned",
                IsSuccess = true
            };
            var serviceMock = new Mock<IUserService>();
            serviceMock.Setup(r => r.CreateUserRoleAsync(roleModel)).ReturnsAsync(userResponseModel);

            //ACT
            var authController = new AuthController(serviceMock.Object);
            var response = await authController.CreateUserRolenAsync(roleModel);
            var status = response.Result as OkObjectResult;
            var userResponse = status.Value as UserManagerResponse;

            //ASSERT
            Assert.True(userResponse.IsSuccess);
        }

        //tc2
        [Fact]
        public async Task CreateUserRole_ReturnsBadRequestResultNotSuccess()
        {
            //ARRANGE
            var roleModel = new CreateUserRoleViewModel()
            {
                UserId = "a391ce2932cd-19ca392f32c2",
                RoleId = "3",
            };
            var userResponseModel = new UserManagerResponse()
            {
                Token = "Role does not exist",
                IsSuccess = false
            };
            var serviceMock = new Mock<IUserService>();
            serviceMock.Setup(r => r.CreateUserRoleAsync(roleModel)).ReturnsAsync(userResponseModel);

            //ACT
            var authController = new AuthController(serviceMock.Object);
            var response = await authController.CreateUserRolenAsync(roleModel);
            var status = response.Result as BadRequestObjectResult;
            var userResponse = status.Value as UserManagerResponse;

            //ASSERT
            Assert.False(userResponse.IsSuccess);
        }

        //tc3
        [Fact]
        public async Task CreateUserRole_ReturnsBadRequestInvalidModel()
        {
            //ARRANGE
            var roleModel = new CreateUserRoleViewModel()
            {
                UserId = null,
                RoleId = null
            };
            var serviceMock = new Mock<IUserService>();

            //ACT
            var authController = new AuthController(serviceMock.Object);
            authController.ModelState.AddModelError("test", "modelNotValid");
            var response = await authController.CreateUserRolenAsync(roleModel);
            var actualStatusCode = ((BadRequestObjectResult)response.Result).StatusCode;

            //ASSERT
            Assert.Equal(400, actualStatusCode);
        }

        //LoginAsync
        //tc1
        [Fact]
        public async Task Login_ReturnsStatusCode200()
        {
            //ARRANGE
            var loginModel = new LoginViewModel()
            {
                Email = "testmail@gmail.com",
                Password = "Secret12345",
            };
            var userResponseModel = new UserManagerResponse()
            {
                Token = "tokenToAccesSuperSecret",
                IsSuccess = true,
            };
            var serviceMock = new Mock<IUserService>();
            serviceMock.Setup(r => r.LoginUserAsync(loginModel)).ReturnsAsync(userResponseModel);

            //ACT
            var authController = new AuthController(serviceMock.Object);
            var response = await authController.LoginAsync(loginModel);
            var status = response.Result as OkObjectResult;
            var userResponse = status.Value as UserManagerResponse;

            //ASSERT
            Assert.True(userResponse.IsSuccess);
        }

        //tc2
        [Fact]
        public async Task Login_ReturnsBadRequestResultNotSuccess()
        {
            //ARRANGE
            var loginModel = new LoginViewModel()
            {
                Email = "testmail@gmail.com",
                Password = "thisIsNotThePass"
            };
            var userResponseModel = new UserManagerResponse()
            {
                Token = "Invalid password",
                IsSuccess = false,
            };
            var serviceMock = new Mock<IUserService>();
            serviceMock.Setup(r => r.LoginUserAsync(loginModel)).ReturnsAsync(userResponseModel);

            //ACT
            var authController = new AuthController(serviceMock.Object);
            var response = await authController.LoginAsync(loginModel);
            var status = response.Result as BadRequestObjectResult;
            var userResponse = status.Value as UserManagerResponse;

            //ASSERT
            Assert.False(userResponse.IsSuccess);
        }

        //tc3
        [Fact]
        public async Task Login_ReturnsBadRequestInvalidModel()
        {
            //ARRANGE
            var loginModel = new LoginViewModel()
            {
                Email = "notAnEmail",
                Password = "a"
            };
            var serviceMock = new Mock<IUserService>();

            //ACT
            var authController = new AuthController(serviceMock.Object);
            authController.ModelState.AddModelError("test", "modelNotValid");
            var response = await authController.LoginAsync(loginModel);
            var actualStatusCode = ((BadRequestObjectResult)response.Result).StatusCode;

            //ASSERT
            Assert.Equal(400, actualStatusCode);
        }
    }
}

