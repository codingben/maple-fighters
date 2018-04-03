using UserProfile.Server.Common;

namespace UserProfile.Service.Application.Components.Interfaces
{
    internal interface IUserProfilePropertiesChangesNotifier
    {
        void Notify(UserProfilePropertiesChangedEventParameters parameters);
    }
}