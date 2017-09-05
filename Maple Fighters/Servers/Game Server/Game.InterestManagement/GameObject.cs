using CommonTools.Log;
using MathematicsHelper;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;

namespace Game.InterestManagement
{
    public class GameObject : IContainer<GameObjectComponent>, IGameObject
    {
        public int Id { get; }
        public IScene Scene { get; set; }

        IContainer<GameObjectComponent> IGameObject.Entity { get; } = new Container<GameObjectComponent>();

        public GameObject(Vector2 position, Vector2 interestArea)
        {
            var idGenerator = ServerComponents.Container.GetComponent<IdGenerator>().AssertNotNull();
            Id = idGenerator.GenerateId();

            AddComponent(new Transform(position));
            AddComponent(new InterestArea(interestArea));
        }

        public void AddComponent(GameObjectComponent component)
        {
            component.Awake(this);

            ((IGameObject)this).Entity.AddComponent(component);
        }

        public void RemoveComponent<T>() 
            where T : IComponent
        {
            ((IGameObject)this).Entity.RemoveComponent<T>();
        }

        public T GetComponent<T>() 
            where T : IComponent
        {
            return ((IGameObject)this).Entity.GetComponent<T>();
        }

        public void RemoveAllComponents()
        {
            ((IGameObject)this).Entity.RemoveAllComponents();
        }

        public void Dispose()
        {
            RemoveAllComponents();
            Scene.RemoveGameObject(this);
        }
    }
}