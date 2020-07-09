using System;
using Common.ComponentModel;
using Game.Application.Objects.Components;
using InterestManagement;

namespace Game.Application.Objects
{
    public class GameObject : IGameObject
    {
        public int Id { get; }

        public string Name { get; }

        public ITransform Transform { get; }

        public IExposedComponents Components { get; }

        public GameObject(int id, string name, IMatrixRegion<IGameObject> region = null)
        {
            Id = id;
            Name = name;
            Transform = new Transform();
            Components = new ComponentsContainer();

            Components.Add(new GameObjectGetter(this));
            var proximityChecker = Components.Add(new ProximityChecker());

            if (region != null)
            {
                proximityChecker.SetMatrixRegion(region);
            }
        }

        public void Dispose()
        {
            ((IDisposable)Components)?.Dispose();
        }
    }
}