namespace UserProfile.Service.Application.Components.Interfaces
{
    internal interface IDatabaseUserProfileExistence
    {
        bool Exists(int userId);
    }
}