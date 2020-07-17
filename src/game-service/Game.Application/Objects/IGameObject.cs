using System;
using Common.ComponentModel;
using InterestManagement;

namespace Game.Application.Objects
{
    public interface IGameObject : ISceneObject, IDisposable
    {
        IComponents Components { get; }
    }
}