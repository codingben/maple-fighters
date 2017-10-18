using ComponentModel.Common;

namespace Database.Common.AccessToken
{
    public interface IDatabaseAccessTokenProvider : IExposableComponent
    {
        string GetAccessToken(int userId);
    }
}