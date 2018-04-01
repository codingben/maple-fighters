namespace UserProfile.Service.Application.Components
{
    internal interface IDatabaseUserProfileExistence
    {
        bool Exists(int userId);
    }
}