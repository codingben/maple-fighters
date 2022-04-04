using System;
using Authenticator.Domain.Repository;

namespace Authenticator.Domain.Aggregates.User
{
    public interface IAccount : IEntity<string>
    {
        string Email { get; set; }

        string Password { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        DateTime CreationDate { get; set; }
    }
}