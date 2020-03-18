using Authenticator.API.Datas;
using Authenticator.Domain.Aggregates.User.Services;

namespace Authenticator.API.Controllers
{
    public interface IRegistrationController
    {
        AccountCreationStatus Register(RegistrationData registrationData);
    }
}