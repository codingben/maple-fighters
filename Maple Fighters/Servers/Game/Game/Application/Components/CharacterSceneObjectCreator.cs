using CommonTools.Log;
using ComponentModel.Common;
using Game.Application.SceneObjects.Components;
using Game.InterestManagement;
using MathematicsHelper;
using ServerApplication.Common.ApplicationBase;
using Shared.Game.Common;
using SceneObject = Game.InterestManagement.SceneObject;

namespace Game.Application.Components
{
    internal class CharacterSceneObjectCreator : Component<IServerEntity>, ICharacterSceneObjectCreator
    {
        private const string SCENE_OBJECT_NAME = "Player";
        private ISceneContainer sceneContainer;

        protected override void OnAwake()
        {
            base.OnAwake();

            sceneContainer = Entity.Container.GetComponent<ISceneContainer>().AssertNotNull();
        }

        public ISceneObject Create(Character character)
        {
            const Maps MAP = Maps.Map_1;
            var position = new Vector2(18, -1.75f);

            var scene = sceneContainer.GetSceneWrapper(MAP).GetScene().AssertNotNull();
            var sceneObject = scene.AddSceneObject(new SceneObject(SCENE_OBJECT_NAME, position)).AssertNotNull();
            sceneObject.Container.AddComponent(new InterestArea(position, scene.RegionSize));
            sceneObject.Container.AddComponent(new CharacterInformationProvider(character));
            return sceneObject;
        }
    }
}