using ComponentModel.Common;
using Game.Application.PeerLogic.Components.Interfaces;
using Game.Application.GameObjects;

namespace Game.Application.PeerLogic.Components
{
    internal class PlayerGameObjectGetter : Component, IPlayerGameObjectGetter
    {
        private readonly PlayerGameObject playerGameObject;

        public PlayerGameObjectGetter(PlayerGameObject playerGameObject)
        {
            this.playerGameObject = playerGameObject;
        }

        public PlayerGameObject GetPlayerGameObject()
        {
            return playerGameObject;
        }
    }
}