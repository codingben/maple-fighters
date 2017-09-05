using ServerApplication.Common.ComponentModel;

namespace Shared.ServerApplication.Common.PeerLogic
{
    public class EntityWrapper : IContainer<EntityComponent>, IEntity
    {
        public int Id { get; }

        IContainer<EntityComponent> IEntity.Components { get; } = new Container<EntityComponent>();

        public EntityWrapper(int id)
        {
            Id = id;
        }

        public void AddComponent(EntityComponent component)
        {
            component.Awake(this);

            ((IEntity)this).Components.AddComponent(component);
        }

        public void RemoveComponent<T>()
            where T : IComponent
        {
            ((IEntity)this).Components.RemoveComponent<T>();
        }

        public T GetComponent<T>()
            where T : IComponent
        {
            return ((IEntity)this).Components.GetComponent<T>();
        }

        public void RemoveAllComponents()
        {
            ((IEntity)this).Components.RemoveAllComponents();
        }

        public void Dispose()
        {
            RemoveAllComponents();
        }
    }
}