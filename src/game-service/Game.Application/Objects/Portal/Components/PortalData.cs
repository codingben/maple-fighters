using Game.Application.Components;

namespace Game.Application.Objects.Components
{
    public class PortalData : ComponentBase, IPortalData
    {
        private readonly byte map;

        public PortalData(byte map)
        {
            this.map = map;
        }

        public byte GetMap()
        {
            return map;
        }
    }
}