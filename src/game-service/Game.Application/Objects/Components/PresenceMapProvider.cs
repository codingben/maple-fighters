using Common.ComponentModel;

namespace Game.Application.Objects.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class PresenceMapProvider : ComponentBase, IPresenceMapProvider
    {
        private byte map;

        public PresenceMapProvider(byte map = 0)
        {
            this.map = map;
        }

        public void SetMap(byte map)
        {
            this.map = map;
        }

        public byte GetMap()
        {
            return map;
        }
    }
}