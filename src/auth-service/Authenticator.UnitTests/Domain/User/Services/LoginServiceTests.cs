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
    public class LoginServiceTests
    {
        [Fact]
        public void FindAccount_Returns_Account()
        {
            // Arrange
            var dummyAccount = Account.Create(
                email: "benzuk@gmail.com",
                password: "benzuk",
                firstName: "Ben",
                lastName: "Ukhanov");
            var accountRepository = Substitute.For<IAccountRepository>();
            accountRepository
                .Read(Arg.Any<Expression<Func<IAccount, bool>>>())
                .Returns(dummyAccount);
            var loginService = new LoginService(accountRepository);

            // Act
            var account = loginService.FindAccount("benzuk@gmail.com");

            // Assert
            account.ShouldNotBeNull();
        }
    }
}