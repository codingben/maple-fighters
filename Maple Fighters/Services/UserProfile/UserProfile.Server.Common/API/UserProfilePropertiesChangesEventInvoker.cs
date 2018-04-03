using System;
using ComponentModel.Common;

namespace UserProfile.Server.Common
{
    public class UserProfilePropertiesChangesEventInvoker : Component, IUserProfilePropertiesChangesEventInvoker
    {
        public event Action<UserProfilePropertiesChangedEventParameters> UserProfilePropertiesChanged;

        public void Invoke(UserProfilePropertiesChangedEventParameters parameters)
        {
            UserProfilePropertiesChanged?.Invoke(parameters);
        }
    }
}