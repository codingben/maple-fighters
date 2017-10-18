using ComponentModel.Common;

namespace Login.Application.Components
{
    internal interface IDatabaseUserVerifier : IExposableComponent
    {
        bool IsExists(string email);
    }
}