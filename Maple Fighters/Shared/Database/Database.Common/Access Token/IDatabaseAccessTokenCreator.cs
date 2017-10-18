using ComponentModel.Common;

namespace Database.Common.AccessToken
{
    public interface IDatabaseAccessTokenCreator : IExposableComponent
    {
        string Create(int userId);
    }
}