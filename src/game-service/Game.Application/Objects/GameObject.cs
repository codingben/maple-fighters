using System;
using Common.ComponentModel;
using InterestManagement;

namespace Game.Application.Objects
{
    public class GameObject : IGameObject
    {
        public int Id { get; }

        public string Name { get; }

        public ITransform Transform => new Transform();

        public IExposedComponents Components => new ComponentsContainer();

        public GameObject(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void Dispose()
        {
            ((IDisposable)Components)?.Dispose();
        }
    }
}