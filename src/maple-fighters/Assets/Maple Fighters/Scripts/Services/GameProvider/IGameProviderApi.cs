using System;

namespace Scripts.Services.GameProviderApi
{
    public interface IGameProviderApi
    {
        Action<long, string> GetGamesCallback { get; set; }

        void GetGames();
    }
}