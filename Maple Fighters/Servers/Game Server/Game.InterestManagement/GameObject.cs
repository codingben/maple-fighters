using CommonTools.Log;
using MathematicsHelper;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;

namespace Game.InterestManagement
{
    public class GameObject : IContainer<IGameObject>, IGameObject
    {
        public int Id { get; }
        public IScene Scene { get; set; }

        IContainer IGameObject.Components { get; } = new Container<Component<IGameObject>>();

        public GameObject(Vector2 position, Vector2 interestArea)
        {
            var idGenerator = ServerComponents.Container.GetComponent<IdGenerator>().AssertNotNull();
            Id = idGenerator.GenerateId();

            AddComponent(new Transform(position));
            AddComponent(new InterestArea(interestArea));
        }

        public T AddComponent<T>(T component)
            where T : Component<IGameObject>, IComponent
        {
            component.Awake(this);
            ((IGameObject)this).Components.AddComponent(component);
            return component;
        }

        public void RemoveComponent<T>() 
            where T : IComponent
        {
            ((IGameObject)this).Components.RemoveComponent<T>();
        }

        public T GetComponent<T>() 
            where T : IComponent
        {
            return ((IGameObject)this).Components.GetComponent<T>();
        }

        public void RemoveAllComponents()
        {
            ((IGameObject)this).Components.RemoveAllComponents();
        }

        public void Dispose()
        {
            RemoveAllComponents();
            Scene.RemoveGameObject(this);
        }
    }
}