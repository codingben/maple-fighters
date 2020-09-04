using Authenticator.API.Controllers;
using Authenticator.API.Datas;
using Authenticator.Domain.Aggregates.User;
using Authenticator.Domain.Aggregates.User.Services;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Authenticator.UnitTests.API.Controllers
{
    public class SignUpControllerTests
    {
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
            var registrationService = Substitute.For<IRegistrationService>();
            var signUpController =
                new SignUpController(registrationService);
            var registrationData = new RegistrationData()
            {
                Email = email,
                Password = password,
                FirstName = firstName,
                LastName = lastName
            };

            // Act
            var authenticationStatus =
                signUpController.Register(registrationData);

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

            var registrationService = Substitute.For<IRegistrationService>();
            registrationService
                .CreateAccount(Arg.Any<Account>())
                .Returns(AccountCreationStatus.Succeed);

            var signUpController =
                new SignUpController(registrationService);
            var registrationData = new RegistrationData()
            {
                Email = Email,
                Password = Password,
                FirstName = FirstName,
                LastName = LastName
            };

            // Act
            var authenticationStatus =
                signUpController.Register(registrationData);

            // Assert
            authenticationStatus
                .Equals(AccountCreationStatus.Succeed)
                .ShouldBeTrue();
        }
    }
}