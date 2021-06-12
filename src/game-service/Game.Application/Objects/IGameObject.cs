using System;
using Common.ComponentModel;
using InterestManagement;

namespace Game.Application.Objects
{
    public interface IGameObject : ISceneObject, IDisposable
    {
        string Name { get; }

        IComponents Components { get; }
    }
}