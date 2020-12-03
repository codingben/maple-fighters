namespace UserProfile.Server.Common
{
    public interface IUserProfileServiceAPI
    {
        void SubscribeToUserProfile(int userId);
        void UnsubscribeFromUserProfile(int userId);
        void ChangeUserProfileProperties(ChangeUserProfilePropertiesRequestParameters parameters);
    }
}