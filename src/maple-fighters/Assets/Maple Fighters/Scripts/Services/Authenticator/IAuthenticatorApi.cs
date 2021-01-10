using System;

namespace Scripts.Services.AuthenticatorApi
{
    public interface IAuthenticatorApi
    {
        Action<long, string> LoginCallback { get; set; }

        Action<long, string> RegisterCallback { get; set; }

        void Login(string email, string password);

        void Register(string email, string password, string firstname, string lastname);
    }
}