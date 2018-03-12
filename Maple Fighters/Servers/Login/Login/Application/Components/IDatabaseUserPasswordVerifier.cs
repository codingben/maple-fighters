using ComponentModel.Common;

namespace Login.Application.Components
{
    internal interface IDatabaseUserPasswordVerifier : IExposableComponent
    {
        bool Verify(string email, string password);
    }
}