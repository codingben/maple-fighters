using ServerApplication.Common.ComponentModel;

namespace Shared.ServerApplication.Common.PeerLogic
{
    public class EntityWrapper : IContainer<IPeerEntity>, IPeerEntity
    {
        IContainer IPeerEntity.Components { get; } = new Container<Component<IPeerEntity>>();

        public void AddComponent(Component<IPeerEntity> component)
        {
            component.Awake(this);

            ((IPeerEntity)this).Components.AddComponent(component);
        }

        public T AddComponent<T>(T component) 
            where T : Component<IPeerEntity>, IComponent
        {
            component.Awake(this);
            ((IPeerEntity)this).Components.AddComponent(component);
            return component;
        }

        public void RemoveComponent<T>()
            where T : IComponent
        {
            ((IPeerEntity)this).Components.RemoveComponent<T>();
        }

        public T GetComponent<T>()
            where T : IComponent
        {
            return ((IPeerEntity)this).Components.GetComponent<T>();
        }

        public void RemoveAllComponents()
        {
            ((IPeerEntity)this).Components.RemoveAllComponents();
        }

        public void Dispose()
        {
            RemoveAllComponents();
        }
    }
}