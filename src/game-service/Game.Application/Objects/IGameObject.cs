using Common.ComponentModel;
using InterestManagement;

namespace Game.Application.Objects
{
    public interface IGameObject : ISceneObject
    {
        IExposedComponents Components { get; }
    }
}