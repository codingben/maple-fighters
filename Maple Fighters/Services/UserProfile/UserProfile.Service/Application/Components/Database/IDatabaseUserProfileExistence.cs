using ComponentModel.Common;

namespace UserProfile.Service.Application.Components
{
    internal interface IDatabaseUserProfileExistence : IExposableComponent
    {
        bool Exists(int userId);
    }
}