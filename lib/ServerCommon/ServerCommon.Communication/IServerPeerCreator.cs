using System.Threading.Tasks;
using CommonTools.Coroutines;

namespace ServerCommon.Communication
{
    public interface IServerPeerCreator
    {
        Task ConnectAsync(IYield yield, string ip, int port);
    }
}