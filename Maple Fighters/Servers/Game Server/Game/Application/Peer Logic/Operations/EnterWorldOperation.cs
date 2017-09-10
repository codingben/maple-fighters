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
    internal class EnterWorldOperation : IOperationRequestHandler<EmptyParameters, EnterWorldOperationResponseParameters>
    {
        private readonly int peerId;
        private readonly Action<IGameObject> onAuthenticated;

        public EnterWorldOperation(int peerId, Action<IGameObject> onAuthenticated)
        {
            this.peerId = peerId;
            this.onAuthenticated = onAuthenticated;
        }

        public EnterWorldOperationResponseParameters? Handle(MessageData<EmptyParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var sceneId = 1;
            var position = new Vector2(0, -6);
            var interestArea = new Vector2(5, 2.5f);
            var playerGameObject = CreatePlayerGameObject(sceneId, position, interestArea);

            onAuthenticated.Invoke(playerGameObject);

            var entityTemp = new Entity(playerGameObject.Id, EntityType.Player);
            return new EnterWorldOperationResponseParameters(entityTemp, position.X, position.Y);
        }

        private IGameObject CreatePlayerGameObject(int sceneId, Vector2 position, Vector2 interestArea)
        {
            var scene = Server.Entity.Container.GetComponent<SceneContainer>().AssertNotNull();
            var playerObject = scene?.GetScene(sceneId)?.AddGameObject(new GameObject(peerId, position, interestArea));
            return playerObject;
        }
    }
}