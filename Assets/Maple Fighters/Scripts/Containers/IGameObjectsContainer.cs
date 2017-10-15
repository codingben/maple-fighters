using Scripts.Gameplay;

namespace Scripts.Containers
{
    public interface IGameObjectsContainer
    {
        IGameObject GetLocalGameObject();
        IGameObject GetRemoteGameObject(int id);
    }
}