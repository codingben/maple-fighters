using ComponentModel.Common;

namespace Database.Common.AccessToken
{
    public interface ILocalDatabaseAccessTokens : IExposableComponent
    {
        void Add(int peerId, string accessToken);

        bool Exists(string accessToken);
    }
}