using System;

namespace Scripts.Services.AuthenticatorApi
{
    public interface IAuthenticatorApi
    {
        Action<long, string> Authentication { get; set; }

        Action<long, string> Registration { get; set; }

        void Authenticate(string email, string password);

        void Register(string email, string password, string firstname, string lastname);
    }
}