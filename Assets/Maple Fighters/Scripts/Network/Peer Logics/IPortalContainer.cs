using Game.Common;

namespace Scripts.Services
{
    public interface IPortalContainer
    {
        void Add(int id, Maps map);
        void Remove(int id);

        Maps GetMap(int id);
    }
}