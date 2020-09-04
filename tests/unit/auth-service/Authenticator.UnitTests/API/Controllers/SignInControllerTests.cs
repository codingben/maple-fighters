using Authenticator.API.Controllers;
using Authenticator.API.Datas;
using Authenticator.Domain.Aggregates.User.Services;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Authenticator.UnitTests.API.Controllers
{
    public class SignInControllerTests
    {
        [Theory]
        [InlineData("benzuk@gmail.com", "")]
        [InlineData("", "benzuk")]
        public void Login_Returns_Failed(string email, string password)
        {
            // Arrange
            var loginService = Substitute.For<ILoginService>();
            var signInController = new SignInController(loginService);
            var loginData = new LoginData(email, password);

            // Act
            var authenticationStatus = signInController.Login(loginData);

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

            var signInController = new SignInController(loginService);
            var loginData = new LoginData(Email, Password);

            // Act
            var authenticationStatus = signInController.Login(loginData);

            // Assert
            authenticationStatus
                .Equals(AuthenticationStatus.Authenticated)
                .ShouldBeTrue();
        }
    }
}