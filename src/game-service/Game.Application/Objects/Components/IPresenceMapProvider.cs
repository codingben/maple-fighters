namespace Game.Application.Objects.Components
{
    public interface IPresenceMapProvider
    {
        void SetMap(byte map);

        byte GetMap();
    }
}