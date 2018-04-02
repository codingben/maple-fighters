using System.Collections.Generic;

namespace PhotonControl.Components.Interfaces
{
    internal interface IServersContainer
    {
        void AddServer(string serverName);
        void RemoveServer(string serverName);

        int GetNumberOfServers();
        ServerDetails GetServerDetails(string serverName);
        IEnumerable<ServerDetails> GetAllServersDetails();
    }
}