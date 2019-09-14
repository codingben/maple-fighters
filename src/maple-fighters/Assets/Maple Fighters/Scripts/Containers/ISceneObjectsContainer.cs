using Game.Common;
using Scripts.Gameplay;

namespace Scripts.Containers
{
    public interface ISceneObjectsContainer
    {
        void SetLocalSceneObject(ISceneObject sceneObject);

        ISceneObject AddSceneObject(SceneObjectParameters parameters);

        void RemoveSceneObject(int id);
    }
}