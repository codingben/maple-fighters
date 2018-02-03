using System;
using ComponentModel.Common;
using MathematicsHelper;

namespace Game.InterestManagement
{
    public interface IScene : IEntity, IDisposable
    {
        IContainer Components { get; }

        Vector2 RegionSize { get; }
        IRegion[,] GetAllRegions();

        ISceneObject AddSceneObject(ISceneObject sceneObject);
        void RemoveSceneObject(int id);

        ISceneObject GetSceneObject(int sceneObjectId);
    }
}