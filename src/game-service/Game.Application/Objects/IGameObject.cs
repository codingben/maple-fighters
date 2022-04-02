using System;
using Game.Application.Components;
using InterestManagement;

namespace Game.Application.Objects
{
    public interface IGameObject : ISceneObject, IDisposable
    {
        string Name { get; }

        IComponents Components { get; }
    }
}