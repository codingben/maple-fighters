using Common.ComponentModel;

namespace Game.Application.Objects.Components
{
    public class PortalData : ComponentBase, IPortalData
    {
        private byte map;

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