using CommonTools.Log;
using ComponentModel.Common;
using Game.InterestManagement;
using PeerLogic.Common;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Components
{
    internal class CharacterGetter : Component<IPeerEntity>, ICharacterGetter
    {
        private readonly ISceneObject sceneObject;
        private readonly Character character;

        public CharacterGetter(ISceneObject sceneObject, Character character)
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