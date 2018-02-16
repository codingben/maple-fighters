using System;
using CommonCommunicationInterfaces;
using ServerApplication.Common.Components;

namespace Server2.Common
{
    public class Server2Service : ServiceBase<ServerOperations, ServerEvents>, IServer2Service
    {
        public event Action<EmptyParameters> TestEvent;

        protected override void OnConnected()
        {
            base.OnConnected();

            SetEventHandler(ServerEvents.Server1Event, TestEvent);

            TestOperation();
        }

        protected override void OnDisconnected(DisconnectReason disconnectReason, string s)
        {
            base.OnDisconnected(disconnectReason, s);

            RemoveEventHandler(ServerEvents.Server1Event);
        }

        public void TestOperation()
        {
            SendOperation(ServerOperations.Server1Operation, new EmptyParameters());
        }

        public override PeerConnectionInformation GetPeerConnectionInformation()
        {
            // TODO: Load from an configuration IP and port
            const string IP = "127.0.0.1";
            const int PORT = 4535;
            return new PeerConnectionInformation(IP, PORT);
        }
    }
}