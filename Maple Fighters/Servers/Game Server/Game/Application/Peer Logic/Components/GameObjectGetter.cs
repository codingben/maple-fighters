using CommonTools.Log;
using Game.InterestManagement;
using ServerApplication.Common.ComponentModel;
using Shared.ServerApplication.Common.PeerLogic;

namespace Game.Application.PeerLogic.Components
{
    internal class GameObjectGetter : Component<IPeerEntity>
    {
        private readonly IGameObject gameObject;

        public GameObjectGetter(IGameObject gameObject)
        {
            this.gameObject = gameObject.AssertNotNull();
        }

        public IGameObject GetGameObject()
        {
            return gameObject;
        }
    }
}