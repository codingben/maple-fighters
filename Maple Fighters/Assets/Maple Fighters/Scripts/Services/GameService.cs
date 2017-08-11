using CommonCommunicationInterfaces;
using Shared.Game.Common;

namespace Scripts.Services
{
    public class GameService : ServiceBase
    {
        protected override void Initiate()
        {
            Connect();
        }

        protected override void OnConnected()
        {
            base.OnConnected();

            PhotonPeer.Send(
                new MessageData<TestRequestParameters>((byte)GameOperations.Test, new TestRequestParameters(5)),
                MessageSendOptions.DefaultReliable());
        }
    }
}