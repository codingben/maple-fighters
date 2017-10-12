using CommonTools.Log;
using Game.Application.GameObjects.Components;
using Game.InterestManagement;
using MathematicsHelper;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;
using Shared.Game.Common;

namespace Game.Application.Components
{
    internal class PlayerGameObjectCreator : Component<IServerEntity>
    {
        private const string GAME_OBJECT_NAME = "Player";
        private SceneContainer sceneContainer;

        protected override void OnAwake()
        {
            base.OnAwake();

            sceneContainer = Server.Entity.Container.GetComponent<SceneContainer>().AssertNotNull();
        }

        public IGameObject Create(Character character, Maps map, Vector2 position)
        {
            var scene = sceneContainer.GetGameSceneWrapper(map).GetScene().AssertNotNull();
            var gameObject = scene.AddGameObject(new InterestManagement.GameObject(GAME_OBJECT_NAME, scene, position)).AssertNotNull();
            gameObject.Container.AddComponent(new InterestArea(position, scene.RegionSize));
            gameObject.Container.AddComponent(new CharacterInformationProvider(character));
            return gameObject;
        }
    }
}