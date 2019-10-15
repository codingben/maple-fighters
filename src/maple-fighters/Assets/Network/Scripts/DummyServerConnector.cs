using System.Threading.Tasks;
using ClientCommunicationInterfaces;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using ExitGames.Client.Photon;

namespace Network.Scripts
{
    public class DummyServerConnector : IServerConnector
    {
        public async Task<IServerPeer> Connect(IYield yield, PeerConnectionInformation connectionInformation, ConnectionProtocol connectionProtocol)
        {
            return await Task.FromResult(new DummyPeer());
        }
    }
}