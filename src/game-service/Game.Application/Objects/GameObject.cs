using System.Collections.Generic;
using Common.MathematicsHelper;
using Game.Application.Components;
using InterestManagement;

namespace Game.Application.Objects
{
    public class GameObject : IGameObject
    {
        public int Id { get; }

        public string Name { get; }

        public ITransform Transform { get; }

        public IComponents Components { get; }

        public GameObject(
            int id,
            string name,
            Vector2 position,
            Vector2 size,
            IComponent[] components = null)
        {
            Id = id;
            Name = name;
            Transform = new Transform(position, size);

            var collection = new List<IComponent>
            {
                new GameObjectGetter(this),
                new ProximityChecker()
            };

            if (components != null)
            {
                collection.AddRange(components);
            }

            Components = new ComponentCollection(collection);
        }

        public void Dispose()
        {
            Components?.Dispose();
        }
    }
}