using ComponentModel.Common;

namespace Database.Common.AccessToken
{
    public interface IDatabaseAccessTokenExistence : IExposableComponent
    {
        bool Exists(string accessToken);
    }
}