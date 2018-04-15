using UnityEngine;

namespace InterestManagement.Scripts
{
    public interface IScene
    {
        Vector2 RegionSize { get; }
        IRegion[,] GetAllRegions();

        ISceneObject AddSceneObject(ISceneObject sceneObject);
        void RemoveSceneObject(ISceneObject sceneObject);

        ISceneObject GetSceneObject(int id);
    }
}