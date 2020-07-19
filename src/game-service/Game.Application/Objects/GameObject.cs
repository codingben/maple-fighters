using System.Collections.Generic;
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

        public GameObject(int id, string name, IComponent[] components = null)
        {
            Id = id;
            Name = name;
            Transform = new Transform();
            Components = new ComponentCollection(UnionCommonComponents(components));
        }

        public void Dispose()
        {
            Components?.Dispose();
        }

        private IEnumerable<IComponent> UnionCommonComponents(IComponent[] components = null)
        {
            yield return new GameObjectGetter(this);
            yield return new ProximityChecker();

            if (components != null)
            {
                foreach (var component in components)
                {
                    yield return component;
                }
            }
        }
    }
}