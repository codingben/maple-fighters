using Common.ComponentModel;
using InterestManagement;

namespace Game.Application.Objects
{
    public class GameObject : ISceneObject
    {
        public int Id { get; }

        public string Name { get; }

        public ITransform Transform => new Transform();

        public IComponents Components => new ComponentsContainer();

        public GameObject(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}