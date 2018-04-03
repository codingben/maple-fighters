using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using ComponentModel.Common;
using GameServerProvider.Service.Application.Components.Interfaces;

namespace GameServerProvider.Service.Application.Components
{
    internal class GameServersInformationStorage : Component, IGameServerInformationCreator, IGameServerInformationRemover, IGameServersInformationProvider
    {
        private readonly object locker = new object();
        private readonly Dictionary<int, GameServerInformation> gameServersInformation = new Dictionary<int, GameServerInformation>();

        protected override void OnDestroy()
        {
            base.OnDestroy();

            lock (locker)
            {
                gameServersInformation.Clear();
            }
        }

        public void Add(int id, GameServerInformation gameServerInformation)
        {
            lock (locker)
            {
                if (gameServersInformation.ContainsKey(id))
                {
                    LogUtils.Log(MessageBuilder.Trace($"A game server with id #{id} already exists in a storage."));
                    return;
                }

                LogUtils.Log($"Added a game server with id #{id}");

                gameServersInformation.Add(id, gameServerInformation);
            }
        }

        public void Remove(int id)
        {
            lock (locker)
            {
                if (!gameServersInformation.ContainsKey(id))
                {
                    LogUtils.Log(MessageBuilder.Trace($"A game server with id #{id} does not exist in a storage."));
                    return;
                }

                LogUtils.Log($"Removed a game server with id #{id}");

                gameServersInformation.Remove(id);
            }
        }

        public IEnumerable<GameServerInformation> Provide()
        {
            lock (locker)
            {
                return gameServersInformation.Values.ToArray();
            }
        }

        public GameServerInformation? Provide(int id)
        {
            lock (locker)
            {
                if (gameServersInformation.TryGetValue(id, out var gameServerInformation))
                {
                    return gameServerInformation;
                }

                LogUtils.Log(MessageBuilder.Trace($"Could not find a game server with id #{id}"));
                return null;
            }
        }
    }
}