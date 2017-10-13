using Game.InterestManagement;
using ServerApplication.Common.ComponentModel;

namespace Game.Application.PeerLogic.Components
{
    internal class PeerIdGetter : Component<IGameObject>
    {
        private readonly int id;

        public PeerIdGetter(int id)
        {
            this.id = id;
        }

        public int GetId()
        {
            return id;
        }
    }
}