using Game.Application.Components;
using Game.Application.PeerLogic;
using PythonScripting;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;

namespace Game.Application
{
    using Application = ServerApplication.Common.ApplicationBase.Application;

    public class GameApplication : Application
    {
        public GameApplication(IFiberProvider fiberProvider) 
            : base(fiberProvider)
        {
            // Left blank intentionally
        }

        public override void Startup()
        {
            base.Startup();

            AddCommonComponents();
            AddComponents();
        }
        
        public override void OnConnected(IClientPeer clientPeer)
        {
            WrapClientPeer(clientPeer, new UnauthenticatedGamePeerLogic());
        }

        private void AddComponents()
        {
            Server.Entity.Container.AddComponent(new PythonScriptEngine());
            Server.Entity.Container.AddComponent(new SceneContainer());
        }
    }
}