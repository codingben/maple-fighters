using Authenticator.API.Controllers;
using Authenticator.API.Datas;
using Authenticator.Domain.Aggregates.User.Services;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Authenticator.UnitTests.API.Controllers
{
    public class LoginControllerTests
    {
        [Theory]
        [InlineData("benzuk@gmail.com", "")]
        [InlineData("", "benzuk")]
        public void Login_Returns_Failed(string email, string password)
        {
            // Arrange
            var loginService = Substitute.For<ILoginService>();
            var authController = new AuthController(loginService);
            var authenticationData = new AuthenticationData(email, password);

            // Act
            var authenticationStatus =
                authController.Login(authenticationData);

            // Assert
            authenticationStatus
                .Equals(AuthenticationStatus.Failed)
                .ShouldBeTrue();
        }

        [Fact]
        public void Login_Returns_Authenticated()
        {
            // Arrange
            const string Email = "benzuk@gmail.com";
            const string Password = "benzuk";

            var loginService = Substitute.For<ILoginService>();
            loginService
                .Authenticate(Email, Password)
                .Returns(AuthenticationStatus.Authenticated);

            var authController = new AuthController(loginService);
            var authenticationData = new AuthenticationData(Email, Password);

            // Act
            var authenticationStatus =
                authController.Login(authenticationData);

            // Assert
            authenticationStatus
                .Equals(AuthenticationStatus.Authenticated)
                .ShouldBeTrue();
        }
    }
}