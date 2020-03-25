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
        public void CreateAccount_Returns_EmailExists()
        {
            // Arrange
            var account = AccountFactory.CreateAccount(
                "benzuk@gmail.com",
                "benzuk",
                "Ben",
                "Ben Ukhanov");
            var accountRepository = Substitute.For<IAccountRepository>();
            accountRepository
                .Read(Arg.Any<Expression<Func<Account, bool>>>())
                .Returns(account);
            var registrationService = new RegistrationService(accountRepository);

            // Act
            var accountCreationStatus =
                registrationService.CreateAccount(account);

            // Assert
            accountCreationStatus
                .Equals(AccountCreationStatus.EmailExists)
                .ShouldBeTrue();
        }

        [Fact]
        public void CreateAccount_Returns_Succeed()
        {
            // Arrange
            var account = AccountFactory.CreateAccount(
                "benzuk@gmail.com",
                "benzuk",
                "Ben",
                "Ben Ukhanov");
            var accountRepository = Substitute.For<IAccountRepository>();
            var registrationService = new RegistrationService(accountRepository);

            // Act
            var accountCreationStatus =
                registrationService.CreateAccount(account);

            // Assert
            accountCreationStatus
                .Equals(AccountCreationStatus.Succeed)
                .ShouldBeTrue();
        }
    }
}