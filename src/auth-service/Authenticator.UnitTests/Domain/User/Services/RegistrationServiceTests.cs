using System;
using System.Linq.Expressions;
using Authenticator.Domain.Aggregates.User;
using Authenticator.Domain.Aggregates.User.Services;
using Authenticator.Infrastructure;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Authenticator.UnitTests.Domain.User.Services
{
    public class RegistrationServiceTests
    {
        [Fact]
        public void CheckIfEmailExists_Returns_EmailExists()
        {
            // Arrange
            var dummyAccount = Account.Create(
                email: "benzuk@gmail.com",
                password: "benzuk",
                firstName: "Ben",
                lastName: "Ukhanov");
            var accountRepository = Substitute.For<IAccountRepository>();
            accountRepository
                .Read(Arg.Any<Expression<Func<Account, bool>>>())
                .Returns(dummyAccount);
            var registrationService =
                new RegistrationService(accountRepository);

            // Act
            var result =
                registrationService.CheckIfEmailExists("benzuk@gmail.com");

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void CreateAccount_Create_Received()
        {
            // Arrange
            var dummyAccount = Account.Create(
                email: "benzuk@gmail.com",
                password: "benzuk",
                firstName: "Ben",
                lastName: "Ukhanov");
            var accountRepository = Substitute.For<IAccountRepository>();
            var registrationService =
                new RegistrationService(accountRepository);

            // Act
            registrationService.CreateAccount(dummyAccount);

            // Assert
            accountRepository.Received().Create(Arg.Any<Account>());
        }
    }
}