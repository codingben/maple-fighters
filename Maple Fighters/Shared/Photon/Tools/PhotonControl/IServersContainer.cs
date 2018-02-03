using System.Collections.Generic;
using ComponentModel.Common;

namespace PhotonControl
{
    internal interface IServersContainer : IExposableComponent
    {
        void AddServer(string serverName);
        void RemoveServer(string serverName);

        int GetNumberOfServers();
        ServerDetails GetServerDetails(string serverName);
        IEnumerable<ServerDetails> GetAllServersDetails();
    }
}