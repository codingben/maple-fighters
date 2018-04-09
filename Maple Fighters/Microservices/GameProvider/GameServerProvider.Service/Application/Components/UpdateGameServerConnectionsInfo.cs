using CommonTools.Log;
using ComponentModel.Common;
using GameServerProvider.Service.Application.Components.Interfaces;

namespace GameServerProvider.Service.Application.Components
{
    internal class UpdateGameServerConnectionsInfo : Component, IUpdateGameServerConnectionsInfo
    {
        private IGameServerInformationChanger gameServerInformationChanger;
        private IGameServersInformationProvider gameServersInformationProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            gameServerInformationChanger = Components.GetComponent<IGameServerInformationChanger>().AssertNotNull();
            gameServersInformationProvider = Components.GetComponent<IGameServersInformationProvider>().AssertNotNull();
        }

        public void Update(int id, int connections)
        {
            var gameServerInformation = gameServersInformationProvider.Provide(id);
            if (gameServerInformation != null)
            {
                var info = gameServerInformation.Value;
                info.Connections = connections;
                gameServerInformationChanger.Change(id, info);
            }
        }
    }
}