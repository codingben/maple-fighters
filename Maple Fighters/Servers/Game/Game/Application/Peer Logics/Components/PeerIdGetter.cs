using ComponentModel.Common;
using Game.InterestManagement;

namespace Game.Application.PeerLogic.Components
{
    internal class PeerIdGetter : Component<ISceneObject>, IPeerIdGetter
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