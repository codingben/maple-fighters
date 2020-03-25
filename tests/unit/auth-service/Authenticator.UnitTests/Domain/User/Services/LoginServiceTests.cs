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
            var authenticationStatus =
                loginService.Authenticate("benzuk@gmail.com", "benzuk");

            // Assert
            authenticationStatus
                .Equals(AuthenticationStatus.NotFound)
                .ShouldBeTrue();
        }

        [Fact]
        public void Authenticate_Returns_Authenticated()
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
            var loginService = new LoginService(accountRepository);

            // Act
            var authenticationStatus = 
                loginService.Authenticate("benzuk@gmail.com", "benzuk");

            // Assert
            authenticationStatus
                .Equals(AuthenticationStatus.Authenticated)
                .ShouldBeTrue();
        }

        [Fact]
        public void Authenticate_Returns_WrongPassword()
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
            var loginService = new LoginService(accountRepository);

            // Act
            var authenticationStatus =
                loginService.Authenticate("benzuk@gmail.com", "ben");

            // Assert
            authenticationStatus
                .Equals(AuthenticationStatus.WrongPassword)
                .ShouldBeTrue();
        }
    }
}