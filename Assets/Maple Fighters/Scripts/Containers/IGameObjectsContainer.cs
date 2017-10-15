using Scripts.Gameplay;
using Shared.Game.Common;
using GameObject = UnityEngine.GameObject;

namespace Scripts.Containers
{
    public interface IGameObjectsContainer
    {
        void CreateLocalGameObject(Shared.Game.Common.GameObject characterGameObject, Character character);

        IGameObject GetLocalGameObject();

        GameObject GetRemoteGameObject(int id);
    }
}