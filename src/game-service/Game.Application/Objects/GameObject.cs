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

        public IComponents Components { get; }

        public GameObject(int id, string name, IMatrixRegion<IGameObject> region = null)
        {
            Id = id;
            Name = name;
            Transform = new Transform();
            Components = new ComponentCollection(new IComponent[]
            {
                new GameObjectGetter(this),
                new ProximityChecker()
            });

            if (region != null)
            {
                var proximityChecker = Components.Get<IProximityChecker>();
                proximityChecker.SetMatrixRegion(region);
            }
        }

        public void Dispose()
        {
            Components?.Dispose();
        }
    }
}