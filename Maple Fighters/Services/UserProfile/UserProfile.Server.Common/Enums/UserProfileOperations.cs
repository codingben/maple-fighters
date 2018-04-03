namespace UserProfile.Server.Common
{
    public enum UserProfileOperations : byte
    {
        Register,
        Unregister,
        Subscribe,
        Unsubscribe,
        ChangeUserProfileProperties
    }
}