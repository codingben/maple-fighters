using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using ComponentModel.Common;

namespace PhotonControl
{
    internal class ServersContainer : Component, IServersContainer
    {
        private readonly Dictionary<string, ServerDetails> servers = new Dictionary<string, ServerDetails>();
        private IServersMenu serversMenu;

        protected override void OnAwake()
        {
            base.OnAwake();

            serversMenu = Entity.AddComponent(new ServersMenu());
        }

        public void AddServer(string serverName)
        {
            if (servers.ContainsKey(serverName))
            {
                LogUtils.Log($@"{serverName} server already exists in a servers list.");
                return;
            }

            var serverItem = serversMenu.AddServerItemToServersMenu(serverName);
            if (serverItem != null)
            {
                servers.Add(serverName, new ServerDetails(serverItem));
            }
        }

        public void RemoveServer(string serverName)
        {
            if (!servers.ContainsKey(serverName))
            {
                LogUtils.Log($@"{serverName} server does not exist in a servers list.");
                return;
            }

            servers.Remove(serverName);
            serversMenu.RemoveServerItemFromServersMenu(serverName);
        }

        public int GetNumberOfServers()
        {
            return servers.Count;
        }

        public ServerDetails GetServerDetails(string serverName)
        {
            if (servers.TryGetValue(serverName, out var serverDetails))
            {
                return serverDetails;
            }

            LogUtils.Log($@"{serverName} server does not exist in a servers list.");
            return null;
        }

        public IEnumerable<ServerDetails> GetAllServersDetails()
        {
            return servers.Values.ToArray();
        }
    }
}