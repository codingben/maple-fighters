using Authenticator.API.Controllers;
using Authenticator.API.Datas;
using Authenticator.Domain.Aggregates.User;
using Authenticator.Domain.Aggregates.User.Services;
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
        public void Login_Returns_Failed(string email, string password)
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
            var authenticationStatus = authController.Login(loginData);

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

            var registrationService = Substitute.For<IRegistrationService>();
            var loginService = Substitute.For<ILoginService>();
            loginService
                .Authenticate(Email, Password)
                .Returns(AuthenticationStatus.Authenticated);

            var authController =
                new AuthController(loginService, registrationService);
            var loginData = new LoginData()
            {
                Email = Email,
                Password = Password
            };

            // Act
            var authenticationStatus = authController.Login(loginData);

            // Assert
            authenticationStatus
                .Equals(AuthenticationStatus.Authenticated)
                .ShouldBeTrue();
        }

        [Theory]
        [InlineData("", "benzuk", "Ben", "Ukhanov")]
        [InlineData("benzuk@gmail.com", "", "Ben", "Ukhanov")]
        [InlineData("benzuk@gmail.com", "benzuk", "", "Ukhanov")]
        [InlineData("benzuk@gmail.com", "benzuk", "Ben", "")]
        public void Register_Returns_Failed(
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
            var authenticationStatus =
                authController.Register(registrationData);

            // Assert
            authenticationStatus
                .Equals(AccountCreationStatus.Failed)
                .ShouldBeTrue();
        }

        [Fact]
        public void Register_Returns_Succeed()
        {
            // Arrange
            const string Email = "benzuk@gmail.com";
            const string Password = "benzuk";
            const string FirstName = "Ben";
            const string LastName = "Ukhanov";

            var loginService = Substitute.For<ILoginService>();
            var registrationService = Substitute.For<IRegistrationService>();
            registrationService
                .CreateAccount(Arg.Any<Account>())
                .Returns(AccountCreationStatus.Succeed);

            var authController =
                new AuthController(loginService, registrationService);
            var registrationData = new RegistrationData()
            {
                Email = Email,
                Password = Password,
                FirstName = FirstName,
                LastName = LastName
            };

            // Act
            var authenticationStatus =
                authController.Register(registrationData);

            // Assert
            authenticationStatus
                .Equals(AccountCreationStatus.Succeed)
                .ShouldBeTrue();
        }
    }
}