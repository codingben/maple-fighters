using ComponentModel.Common;

namespace Database.Common.AccessToken
{
    public interface IDatabaseUserIdViaAccessTokenProvider : IExposableComponent
    {
        int GetUserId(string accessToken);
    }
}