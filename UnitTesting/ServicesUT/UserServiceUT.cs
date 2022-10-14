using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AthletesRestAPI.Data.Entity;
using AthletesRestAPI.Data.Repository;
using AthletesRestAPI.Exceptionss;
using AthletesRestAPI.Models;
using AthletesRestAPI.Models.Security;
using AthletesRestAPI.Services;
using AthletesRestAPI.Services.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Moq;
using RestaurantRestAPI.Data;

namespace UnitTesting.ServicesUT
{
    public class UserServiceUT
    {
        //LoginUserAsync
        //tc1
        [Fact]
        public async Task LoginUser_DoesntFoundUserWithEmail()
        {
            //ARRANGE
            var loginModel = new LoginViewModel()
            {
                Email = "testmail@gmai.com",
                Password = "Secret1234",
            };

            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
            var configurationMock = new Mock<IConfiguration>();

            userManagerMock.Setup(r => r.FindByEmailAsync(loginModel.Email)).ReturnsAsync((IdentityUser)null);

            //ACT
            var userService = new UserService(userManagerMock.Object, roleManagerMock.Object, configurationMock.Object);
            var response = await userService.LoginUserAsync(loginModel);

            //ASSERT
            Assert.Equal("There is no user with that Email address", response.Token);
        }

        //tc2
        [Fact]
        public async Task LoginUser_UserInvalidPassword()
        {
            //ARRANGE
            var loginModel = new LoginViewModel()
            {
                Email = "testmail@gmai.com",
                Password = "thisIsNotThePass",
            };
            var identityUser = new IdentityUser()
            {
                Id = "12",
                UserName = "TestName",
                Email = "testmail@gmai.com",
                PasswordHash = "hasAFAS2341HES",
            };
            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
            var configurationMock = new Mock<IConfiguration>();

            userManagerMock.Setup(r => r.FindByEmailAsync(loginModel.Email)).ReturnsAsync(identityUser);
            userManagerMock.Setup(r => r.CheckPasswordAsync(identityUser, loginModel.Password)).ReturnsAsync(false);

            //ACT
            var userService = new UserService(userManagerMock.Object, roleManagerMock.Object, configurationMock.Object);
            var response = await userService.LoginUserAsync(loginModel);

            //ASSERT
            Assert.Equal("Invalid password", response.Token);
        }

        //tc3
        [Fact]
        public async Task LoginUser_LoginIsSuccesfulWithRoles()
        {
            //ARRANGE
            var loginModel = new LoginViewModel()
            {
                Email = "correctMail@gmail.com",
                Password = "Secret1234",
            };
            var identityUser = new IdentityUser()
            {
                Id = "12",
                UserName = "TestName",
                Email = "correctMail@gmail.com",
                PasswordHash = "hasAFAS2341HES",
            };
            var roles = new List<String>();
            roles.Add("Admin");
            roles.Add("User");
            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
            var configurationMock = new Mock<IConfiguration>();

            userManagerMock.Setup(r => r.FindByEmailAsync(loginModel.Email)).ReturnsAsync(identityUser);
            userManagerMock.Setup(r => r.CheckPasswordAsync(identityUser, loginModel.Password)).ReturnsAsync(true);
            userManagerMock.Setup(r => r.GetRolesAsync(identityUser)).ReturnsAsync(roles);
            configurationMock.Setup(r => r["AuthSettings:Key"]).Returns("SuperSecretKeyEncryptor");

            //ACT
            var userService = new UserService(userManagerMock.Object, roleManagerMock.Object, configurationMock.Object);
            var response = await userService.LoginUserAsync(loginModel);

            //ASSERT
            Assert.True(response.IsSuccess);
        }

        //tc4
        [Fact]
        public async Task LoginUser_LoginIsSuccesfulWithoutRoles()
        {
            //ARRANGE
            var loginModel = new LoginViewModel()
            {
                Email = "correctMail@gmail.com",
                Password = "Secret1234",
            };
            var identityUser = new IdentityUser()
            {
                Id = "12",
                UserName = "TestName",
                Email = "correctMail@gmail.com",
                PasswordHash = "hasAFAS2341HES",
            };
            var roles = new List<String>();
            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
            var configurationMock = new Mock<IConfiguration>();

            userManagerMock.Setup(r => r.FindByEmailAsync(loginModel.Email)).ReturnsAsync(identityUser);
            userManagerMock.Setup(r => r.CheckPasswordAsync(identityUser, loginModel.Password)).ReturnsAsync(true);
            userManagerMock.Setup(r => r.GetRolesAsync(identityUser)).ReturnsAsync(roles);
            configurationMock.Setup(r => r["AuthSettings:Key"]).Returns("SuperSecretKeyEncryptor");

            //ACT
            var userService = new UserService(userManagerMock.Object, roleManagerMock.Object, configurationMock.Object);
            var response = await userService.LoginUserAsync(loginModel);

            //ASSERT
            Assert.True(response.IsSuccess);
        }

        //RegisterUserAsync
        //tc1
        [Fact]
        public async Task RegisterUser_UserIsNull()
        {
            //ARRANGE
            var registerModel = (RegisterViewModel)null;

            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
            var configurationMock = new Mock<IConfiguration>();


            //ACT
            var userService = new UserService(userManagerMock.Object, roleManagerMock.Object, configurationMock.Object);
            var exception = Assert.ThrowsAsync<NullReferenceException>(async () => await userService.RegisterUserAsync(registerModel));

            //ASSERT
            Assert.Equal("model is null", exception.Result.Message);
        }

        //tc2
        [Fact]
        public async Task RegisterUser_UserPasswordsDontMatch()
        {
            //ARRANGE
            var registerModel = new RegisterViewModel()
            {
                Email = "testmail@gmail.com",
                Password = "SecretPass1234",
                ConfirmPassword = "NoPass1234"
            };

            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
            var configurationMock = new Mock<IConfiguration>();


            //ACT
            var userService = new UserService(userManagerMock.Object, roleManagerMock.Object, configurationMock.Object);
            var response = await userService.RegisterUserAsync(registerModel);

            //ASSERT
            Assert.Equal("Confirm password doesn't match the password", response.Token);
            Assert.False(response.IsSuccess);
        }

        //tc3
        [Fact]
        public async Task RegisterUser_UserRegisterSuccesful()
        {
            //ARRANGE
            var registerModel = new RegisterViewModel()
            {
                Email = "testmail@gmail.com",
                Password = "SecretPass1234",
                ConfirmPassword = "SecretPass1234"
            };

            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
            var configurationMock = new Mock<IConfiguration>();

            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            //ACT
            var userService = new UserService(userManagerMock.Object, roleManagerMock.Object, configurationMock.Object);
            var response = await userService.RegisterUserAsync(registerModel);

            //ASSERT
            Assert.Equal("User created successfully!", response.Token);
            Assert.True(response.IsSuccess);
        }

        //tc4
        [Fact]
        public async Task RegisterUser_UserRegisterFailed()
        {
            //ARRANGE
            var registerModel = new RegisterViewModel()
            {
                Email = "testmail@gmail.com",
                Password = "SecretPass1234",
                ConfirmPassword = "SecretPass1234"
            };
            var errors = new IdentityError[] {
                new IdentityError() { Description="Error 1" },
                new IdentityError() { Description="Error 2" }
            };
            var stringErrors = new String[] { "Error 1", "Error 2" };

            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
            var configurationMock = new Mock<IConfiguration>();

            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(errors));

            //ACT
            var userService = new UserService(userManagerMock.Object, roleManagerMock.Object, configurationMock.Object);
            var response = await userService.RegisterUserAsync(registerModel);

            //ASSERT
            Assert.Equal("User did not create", response.Token);
            Assert.False(response.IsSuccess);
            Assert.Equal(stringErrors, response.Errors);
        }

        //CreateRoleAsync
        //tc1
        [Fact]
        public async Task CreateRole_RoleCreated()
        {
            //ARRANGE
            var roleModel = new CreateRoleViewModel()
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            };

            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
            var configurationMock = new Mock<IConfiguration>();

            roleManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success);

            //ACT
            var userService = new UserService(userManagerMock.Object, roleManagerMock.Object, configurationMock.Object);
            var response = await userService.CreateRoleAsync(roleModel);

            //ASSERT
            Assert.Equal("Role created successfully!", response.Token);
            Assert.True(response.IsSuccess);
        }

        //tc2
        [Fact]
        public async Task CreateRole_RoleFailedToCreate()
        {
            //ARRANGE
            var roleModel = new CreateRoleViewModel()
            {
                Name = "NoRole",
                NormalizedName = "NR"
            };
            var errors = new IdentityError[] {
                new IdentityError() { Description="Error 1" },
                new IdentityError() { Description="Error 2" }
            };
            var stringErrors = new String[] { "Error 1", "Error 2" };

            var userManagerMock = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
            var configurationMock = new Mock<IConfiguration>();


            roleManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Failed(errors));

            //ACT
            var userService = new UserService(userManagerMock.Object, roleManagerMock.Object, configurationMock.Object);
            var response = await userService.CreateRoleAsync(roleModel);

            //ASSERT
            Assert.Equal("Role did not create", response.Token);
            Assert.False(response.IsSuccess);
            Assert.Equal(stringErrors, response.Errors);
        }

        //CreateUserRoleAsync
        //tc1
    }
}
