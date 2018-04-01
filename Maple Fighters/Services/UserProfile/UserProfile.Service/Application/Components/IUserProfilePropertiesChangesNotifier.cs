using UserProfile.Server.Common;

namespace UserProfile.Service.Application.Components
{
    internal interface IUserProfilePropertiesChangesNotifier
    {
        void Notify(UserProfilePropertiesChangedEventParameters parameters);
    }
}