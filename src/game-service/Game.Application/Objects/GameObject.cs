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

        public GameObject(int id, string name)
        {
            Id = id;
            Name = name;
            Transform = new Transform();
            Components = new ComponentCollection();
        }

        public void Dispose()
        {
            Components?.Dispose();
        }
    }
}