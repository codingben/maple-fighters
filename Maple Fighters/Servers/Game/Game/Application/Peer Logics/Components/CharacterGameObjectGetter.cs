using CommonTools.Log;
using Game.InterestManagement;
using ServerApplication.Common.ComponentModel;
using Shared.Game.Common;
using Shared.ServerApplication.Common.PeerLogic;

namespace Game.Application.PeerLogic.Components
{
    internal class CharacterGameObjectGetter : Component<IPeerEntity>
    {
        private readonly IGameObject gameObject;
        private readonly Character character;

        public CharacterGameObjectGetter(IGameObject gameObject, Character character)
        {
            this.gameObject = gameObject.AssertNotNull();
            this.character = character;
        }

        public IGameObject GetGameObject()
        {
            return gameObject;
        }

        public Character GetCharacter()
        {
            return character;
        }
    }
}