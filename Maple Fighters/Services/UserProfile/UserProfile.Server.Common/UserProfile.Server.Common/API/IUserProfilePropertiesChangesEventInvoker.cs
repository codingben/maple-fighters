using System;

namespace UserProfile.Server.Common
{
    public interface IUserProfilePropertiesChangesEventInvoker
    {
        event Action<UserProfilePropertiesChangedEventParameters> UserProfilePropertiesChanged;
        void Invoke(UserProfilePropertiesChangedEventParameters parameters);
    }
}