using ComponentModel.Common;
using InterestManagement.Components.Interfaces;
using MathematicsHelper;

namespace InterestManagement
{
    public interface IScene : IEntity
    {
        Vector2 RegionSize { get; }
        IRegion[,] GetAllRegions();

        ISceneObject AddSceneObject(ISceneObject sceneObject);
        void RemoveSceneObject(int id);

        ISceneObject GetSceneObject(int sceneObjectId);
    }
}