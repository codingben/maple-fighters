using ComponentModel.Common;

namespace Login.Application.Components
{
    internal interface IDatabaseUserIdProvider : IExposableComponent
    {
        int GetUserId(string email);
    }
}