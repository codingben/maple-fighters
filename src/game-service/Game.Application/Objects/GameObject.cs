using System;
using Common.ComponentModel;
using InterestManagement;

namespace Game.Application.Objects
{
    public class GameObject : IGameObject
    {
        public int Id { get; }

        public string Name { get; }

        public ITransform Transform { get; }

        public IExposedComponents Components { get; }

        public GameObject(int id, string name)
        {
            Id = id;
            Name = name;
            Transform = new Transform();
            Components = new ComponentsContainer();
        }

        public void Dispose()
        {
            ((IDisposable)Components)?.Dispose();
        }
    }
}