using System;
using ComponentModel.Common;

namespace Game.InterestManagement
{
    public interface ISceneObject : IEntity, IDisposable
    {
        int Id { get; }
        string Name { get; }

        IContainer<ISceneObject> Components { get; }

        void OnAwake();
        void OnDestroy();
    }
}