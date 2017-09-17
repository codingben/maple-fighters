using System;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.InterestManagement;
using MathematicsHelper;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class EnterWorldOperationHandler : IOperationRequestHandler<EmptyParameters, EnterWorldOperationResponseParameters>
    {
        private readonly int peerId;
        private readonly Action<IGameObject> onAuthenticated;

        public EnterWorldOperationHandler(int peerId, Action<IGameObject> onAuthenticated)
        {
            this.peerId = peerId;
            this.onAuthenticated = onAuthenticated;
        }

        public EnterWorldOperationResponseParameters? Handle(MessageData<EmptyParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var position = new Vector2(18, -5.5f);
            var interestArea = new Vector2(10, 5);
            var gameObject = CreatePlayerGameObject(Maps.Map_1, position, interestArea);

            onAuthenticated.Invoke(gameObject);

            var entityTemp = new Entity(gameObject.Id, EntityType.Player, position.X, position.Y);
            return new EnterWorldOperationResponseParameters(entityTemp, position.X, position.Y);
        }

        private IGameObject CreatePlayerGameObject(Maps map, Vector2 position, Vector2 interestArea)
        {
            var scene = Server.Entity.Container.GetComponent<SceneContainer>().AssertNotNull();
            var playerObject = scene?.GetScene(map)?.AssertNotNull().AddGameObject(new GameObject(map, peerId, position, interestArea));
            return playerObject;
        }
    }
}