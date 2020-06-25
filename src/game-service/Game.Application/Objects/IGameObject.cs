using System;
using Common.ComponentModel;
using InterestManagement;

namespace Game.Application.Objects
{
    public interface IGameObject : ISceneObject, IDisposable
    {
        IExposedComponents Components { get; }
    }
}