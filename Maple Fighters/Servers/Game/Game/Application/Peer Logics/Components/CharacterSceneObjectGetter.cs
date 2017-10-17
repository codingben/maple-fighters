using CommonTools.Log;
using Game.InterestManagement;
using ServerApplication.Common.ComponentModel;
using Shared.Game.Common;
using Shared.ServerApplication.Common.PeerLogic;

namespace Game.Application.PeerLogic.Components
{
    internal class CharacterSceneObjectGetter : Component<IPeerEntity>
    {
        private readonly ISceneObject sceneObject;
        private readonly Character character;

        public CharacterSceneObjectGetter(ISceneObject sceneObject, Character character)
        {
            this.sceneObject = sceneObject.AssertNotNull();
            this.character = character;
        }

        public ISceneObject GetSceneObject()
        {
            return sceneObject;
        }

        public Character GetCharacter()
        {
            return character;
        }
    }
}