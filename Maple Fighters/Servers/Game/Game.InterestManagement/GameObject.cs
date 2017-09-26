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
        public string Name { get; }

        public IScene Scene { get; private set; }

        public IContainer<IGameObject> Container { get; }

        public GameObject(string name, IScene scene, Vector2 position)
        {
            Name = name;

            var idGenerator = Server.Entity.Container.GetComponent<IdGenerator>().AssertNotNull();
            Id = idGenerator.GenerateId();

            Scene = scene;

            Container = new Container<IGameObject>(this);

            Container.AddComponent(new Transform(position));
        }

        public void SetScene(IScene scene)
        {
            RemoveFromScene();

            scene.AddGameObject(this);
            Scene = scene;
        }

        public void RemoveScene()
        {
            Scene = null;
        }

        private void RemoveFromScene()
        {
            Scene.RemoveGameObject(Id);
            Scene = null;
        }

        public void Dispose()
        {
            Container.RemoveAllComponents();
            Scene.RemoveGameObject(Id);
        }
    }
}