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

            AddCommonComponents();

            if (region != null)
            {
                var proximityChecker = Components.Get<IProximityChecker>();
                proximityChecker.SetMatrixRegion(region);
            }
        }

        public void Dispose()
        {
            ((IDisposable)Components)?.Dispose();
        }

        private void AddCommonComponents()
        {
            Components.Add(new GameObjectGetter(this));
            Components.Add(new ProximityChecker());
        }
    }
}