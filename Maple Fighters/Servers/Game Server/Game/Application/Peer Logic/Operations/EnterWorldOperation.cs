using System;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.InterestManagement;
using MathematicsHelper;
using ServerApplication.Common.ComponentModel;
using ServerCommunicationHelper;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class EnterWorldOperation : IOperationRequestHandler<EmptyParameters, EnterWorldOperationResponseParameters>
    {
        private readonly Action<IGameObject> onAuthenticated;

        public EnterWorldOperation(Action<IGameObject> onAuthenticated)
        {
            this.onAuthenticated = onAuthenticated;
        }

        public EnterWorldOperationResponseParameters? Handle(MessageData<EmptyParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var sceneId = 1;
            var position = new Vector2(3.55f, 0.74f);
            var interestArea = new Vector2(5, 2.5f);
            var playerGameObject = CreatePlayerGameObject(sceneId, position, interestArea);

            onAuthenticated.Invoke(playerGameObject);

            var entityTemp = new Shared.Game.Common.Entity(playerGameObject.Id, EntityType.Player);
            return new EnterWorldOperationResponseParameters(entityTemp, position.X, position.Y);
        }

        private IGameObject CreatePlayerGameObject(int sceneId, Vector2 position, Vector2 interestArea)
        {
            var scene = ServerComponents.Container.GetComponent<SceneContainer>().AssertNotNull();
            var playerObject = scene?.GetScene(sceneId)?.AddGameObject(new GameObject(position, interestArea));
            return playerObject;
        }
    }
}