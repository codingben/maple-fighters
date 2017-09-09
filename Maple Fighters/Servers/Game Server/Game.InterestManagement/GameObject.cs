using CommonTools.Log;
using MathematicsHelper;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;

namespace Game.InterestManagement
{
    public class GameObject : IGameObject
    {
        public int Id { get; }
        public IScene Scene { get; set; }
        public IContainer<IGameObject> Container { get; }

        public GameObject(Vector2 position, Vector2 interestArea)
        {
            Container = new Container<IGameObject>(this);

            var idGenerator = Server.Entity.Container.GetComponent<IdGenerator>().AssertNotNull();
            Id = idGenerator.GenerateId();

            Container.AddComponent(new Transform(position));
            Container.AddComponent(new InterestArea(interestArea));
        }

        public void Dispose()
        {
            Container.RemoveAllComponents();
            Scene.RemoveGameObject(this);
        }
    }
}