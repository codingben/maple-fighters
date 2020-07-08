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

        public GameObject(int id, string name)
        {
            Id = id;
            Name = name;
            Transform = new Transform();
            Components = new ComponentsContainer();

            AddCommonComponents();
        }

        public void Dispose()
        {
            ((IDisposable)Components)?.Dispose();
        }

        public void AddProximityChecker(IMatrixRegion<IGameObject> matrixRegion)
        {
            var proximityChecker = Components.Add(new ProximityChecker());
            proximityChecker.SetMatrixRegion(matrixRegion);
        }

        private void AddCommonComponents()
        {
            Components.Add(new GameObjectGetter(this));
        }
    }
}