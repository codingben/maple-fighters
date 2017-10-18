using ComponentModel.Common;

namespace Database.Common.AccessToken
{
    public interface IDatabaseAccessTokenExistenceViaUserId : IExposableComponent
    {
        bool Exists(int userId);
    }
}