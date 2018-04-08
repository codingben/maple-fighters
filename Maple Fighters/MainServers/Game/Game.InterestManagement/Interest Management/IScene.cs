using ComponentModel.Common;
using InterestManagement.Components.Interfaces;
using MathematicsHelper;

namespace InterestManagement
{
    public interface IScene : IEntity
    {
        Vector2 RegionSize { get; }
        IRegion[,] GetAllRegions();

        ISceneObject AddSceneObject(ISceneObject sceneObject, bool onAwake = true);
        void RemoveSceneObject(int id, bool onDestroy = true);

        ISceneObject GetSceneObject(int sceneObjectId);
    }
}