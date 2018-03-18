using System;
using ComponentModel.Common;

namespace UserProfile.Server.Common
{
    public interface IUserProfilePropertiesChangesEventInvoker : IExposableComponent
    {
        event Action<UserProfilePropertiesChangedEventParameters> UserProfilePropertiesChanged;

        void Invoke(UserProfilePropertiesChangedEventParameters parameters);
    }
}