using CommonTools.Log;
using MathematicsHelper;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;
using Shared.Game.Common;

namespace Game.InterestManagement
{
    public class GameObject : IGameObject
    {
        public int Id { get; }
        public IScene Scene { get; private set; }
        public IContainer<IGameObject> Container { get; }

        private SceneContainer sceneContainer;

        public GameObject(Maps map, Vector2 position, Vector2 interestAreaSize)
        {
            var idGenerator = Server.Entity.Container.GetComponent<IdGenerator>().AssertNotNull();
            Id = idGenerator.GenerateId();

            Scene = GetScene(map);

            Container = new Container<IGameObject>(this);

            Container.AddComponent(new Transform(position));
            Container.AddComponent(new InterestArea(position, interestAreaSize));
        }

        public GameObject(Maps map, int id, Vector2 position, Vector2 interestAreaSize)
        {
            Id = id;

            Scene = GetScene(map);

            Container = new Container<IGameObject>(this);

            Container.AddComponent(new Transform(position));
            Container.AddComponent(new InterestArea(position, interestAreaSize));
        }

        public void ChangeScene(Maps map)
        {
            Scene.RemoveGameObjectFromScene(Id);

            var scene = GetScene(map);
            scene.AddGameObject(this);

            Scene = scene;
        }

        private IScene GetScene(Maps map)
        {
            if (sceneContainer == null)
            {
                sceneContainer = Server.Entity.Container.GetComponent<SceneContainer>().AssertNotNull();
            }

            var scene = sceneContainer.GetScene(map).AssertNotNull();
            return scene;
        }

        public void Dispose()
        {
            Container.RemoveAllComponents();
            Scene.RemoveGameObjectFromScene(Id);
        }
    }
}