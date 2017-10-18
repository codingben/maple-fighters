using ComponentModel.Common;

namespace Registration.Application.Components
{
    internal interface IDatabaseUserEmailVerifier : IExposableComponent
    {
        bool Verify(string email);
    }
}