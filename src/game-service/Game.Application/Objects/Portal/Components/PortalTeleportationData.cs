using Game.Application.Components;

namespace Game.Application.Objects.Components
{
    public class PortalTeleportationData : ComponentBase, IPortalTeleportationData
    {
        private readonly byte map;

        public PortalTeleportationData(byte map)
        {
            this.map = map;
        }

        public byte GetDestinationMap()
        {
            return map;
        }
    }
}