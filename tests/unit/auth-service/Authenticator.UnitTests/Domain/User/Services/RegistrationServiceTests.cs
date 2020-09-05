using System;
using System.Linq.Expressions;
using Authenticator.Domain.Aggregates.User;
using Authenticator.Domain.Aggregates.User.Services;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Authenticator.UnitTests.Domain.User.Services
{
    public class RegistrationServiceTests
    {
        [Fact]
        public void CheckIfEmailExists_Returns_Exists()
        {
            // Arrange
            var account = Account.Create(
                "benzuk@gmail.com",
                "benzuk",
                "Ben",
                "Ben Ukhanov");
            var accountRepository = Substitute.For<IAccountRepository>();
            accountRepository
                .Read(Arg.Any<Expression<Func<Account, bool>>>())
                .Returns(account);
            var registrationService =
                new RegistrationService(accountRepository);

            // Act
            var result =
                registrationService.CheckIfEmailExists("benzuk@gmail.com");

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void CreateAccount_Returns_Succeed()
        {
            // Arrange
            var account = Account.Create(
                "benzuk@gmail.com",
                "benzuk",
                "Ben",
                "Ben Ukhanov");
            var accountRepository = Substitute.For<IAccountRepository>();
            var registrationService =
                new RegistrationService(accountRepository);

            // Act
            registrationService.CreateAccount(account);

            // Assert
            accountRepository.Received();
        }
    }
}