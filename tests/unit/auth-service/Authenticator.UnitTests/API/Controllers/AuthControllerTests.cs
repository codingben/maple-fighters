using Authenticator.API.Controllers;
using Authenticator.API.Datas;
using Authenticator.Domain.Aggregates.User;
using Authenticator.Domain.Aggregates.User.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Authenticator.UnitTests.API.Controllers
{
    public class AuthControllerTests
    {
        [Theory]
        [InlineData("benzuk@gmail.com", "")]
        [InlineData("", "benzuk")]
        public void Login_Returns_BadRequest(string email, string password)
        {
            // Arrange
            var loginService = Substitute.For<ILoginService>();
            var registrationService = Substitute.For<IRegistrationService>();
            var authController =
                new AuthController(loginService, registrationService);
            var loginData = new LoginData()
            {
                Email = email,
                Password = password
            };

            // Act
            var actionResult = authController.Login(loginData);

            // Assert
            actionResult.Value.ShouldBeNull();
        }

        [Fact]
        public void Login_Authenticate_Received()
        {
            // Arrange
            var registrationService = Substitute.For<IRegistrationService>();
            var loginService = Substitute.For<ILoginService>();
            var authController =
                new AuthController(loginService, registrationService);
            var loginData = new LoginData()
            {
                Email = "benzuk@gmail.com",
                Password = "benzuk"
            };

            // Act
            authController.Login(loginData);

            // Assert
            loginService.Received().Authenticate(Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData("", "benzuk", "Ben", "Ukhanov")]
        [InlineData("benzuk@gmail.com", "", "Ben", "Ukhanov")]
        [InlineData("benzuk@gmail.com", "benzuk", "", "Ukhanov")]
        [InlineData("benzuk@gmail.com", "benzuk", "Ben", "")]
        public void Register_Returns_BadRequest(
            string email,
            string password,
            string firstName,
            string lastName)
        {
            // Arrange
            var loginService = Substitute.For<ILoginService>();
            var registrationService = Substitute.For<IRegistrationService>();
            var authController =
                new AuthController(loginService, registrationService);
            var registrationData = new RegistrationData()
            {
                Email = email,
                Password = password,
                FirstName = firstName,
                LastName = lastName
            };

            // Act
            var actionResult = authController.Register(registrationData);

            // Assert
            var isBadRequest = actionResult is BadRequestObjectResult;

            isBadRequest.ShouldBeTrue();
        }

        [Fact]
        public void Register_CreateAccount_Received()
        {
            // Arrange
            var loginService = Substitute.For<ILoginService>();
            var registrationService = Substitute.For<IRegistrationService>();
            var authController =
                new AuthController(loginService, registrationService);
            var registrationData = new RegistrationData()
            {
                Email = "benzuk@gmail.com",
                Password = "benzuk",
                FirstName = "ben",
                LastName = "ukhanov"
            };

            // Act
            authController.Register(registrationData);

            // Assert
            registrationService.Received().CreateAccount(Arg.Any<Account>());
        }
    }
}