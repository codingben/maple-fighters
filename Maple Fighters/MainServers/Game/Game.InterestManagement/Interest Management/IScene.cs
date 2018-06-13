using System.Collections.Generic;
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
        bool RemoveSceneObject(ISceneObject sceneObject);

        ISceneObject GetSceneObject(int id);
        IReadOnlyCollection<ISceneObject> GetAllSceneObjects();
    }
}