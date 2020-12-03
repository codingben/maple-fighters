using System;
using System.Linq.Expressions;
using Authenticator.Domain.Aggregates.User;
using Authenticator.Domain.Aggregates.User.Services;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Authenticator.UnitTests.Domain.User.Services
{
    public class LoginServiceTests
    {
        [Fact]
        public void Authenticate_Returns_NotFound()
        {
            // Arrange
            var accountRepository = Substitute.For<IAccountRepository>();
            var loginService = new LoginService(accountRepository);

            // Act
            var account =
                loginService.Authenticate("benzuk@gmail.com", "benzuk");

            // Assert
            account.ShouldBeNull();
        }

        [Fact]
        public void Authenticate_Returns_Account()
        {
            // Arrange
            var dummyAccount = Account.Create(
                "benzuk@gmail.com",
                "benzuk",
                "Ben",
                "Ben Ukhanov");
            var accountRepository = Substitute.For<IAccountRepository>();
            accountRepository
                .Read(Arg.Any<Expression<Func<Account, bool>>>())
                .Returns(dummyAccount);
            var loginService = new LoginService(accountRepository);

            // Act
            var account =
                loginService.Authenticate("benzuk@gmail.com", "benzuk");

            // Assert
            account.ShouldNotBeNull();
        }

        [Fact]
        public void Authenticate_Returns_No_Account()
        {
            // Arrange
            var dummyAccount = Account.Create(
                "benzuk@gmail.com",
                "benzuk",
                "Ben",
                "Ben Ukhanov");
            var accountRepository = Substitute.For<IAccountRepository>();
            accountRepository
                .Read(Arg.Any<Expression<Func<Account, bool>>>())
                .Returns(dummyAccount);
            var loginService = new LoginService(accountRepository);

            // Act
            var account =
                loginService.Authenticate("benzuk@gmail.com", "ben");

            // Assert
            account.ShouldBeNull();
        }
    }
}