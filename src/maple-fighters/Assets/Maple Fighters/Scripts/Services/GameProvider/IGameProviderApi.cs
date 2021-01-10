using System;

namespace Scripts.Services.GameProviderApi
{
    public interface IGameProviderApi
    {
        Action<long, string> GamesCallback { get; set; }

        void Games();
    }
}