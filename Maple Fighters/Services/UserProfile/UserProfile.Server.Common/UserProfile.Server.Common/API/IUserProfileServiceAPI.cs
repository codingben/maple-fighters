using ComponentModel.Common;

namespace UserProfile.Server.Common
{
    public interface IUserProfileServiceAPI : IExposableComponent
    {
        void SubscribeToUserProfile(int userId);
        void UnsubscribeFromUserProfile(int userId);
        void ChangeUserProfileProperties(ChangeUserProfilePropertiesRequestParameters parameters);
    }
}