using System;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.Components;
using Game.Application.PeerLogic.Components;
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
        private readonly SceneContainer sceneContainer;

        public EnterWorldOperationHandler(int peerId, Action<IGameObject> onAuthenticated)
        {
            this.peerId = peerId;
            this.onAuthenticated = onAuthenticated;

            sceneContainer = Server.Entity.Container.GetComponent<SceneContainer>().AssertNotNull();
        }

        public EnterWorldOperationResponseParameters? Handle(MessageData<EmptyParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var playerGameObject = CreatePlayerGameObject();
            playerGameObject.Container.AddComponent(new PeerIdGetter(peerId));

            onAuthenticated.Invoke(playerGameObject);

            var transform = playerGameObject.Container.GetComponent<Transform>().AssertNotNull();
            var gameObject = new Shared.Game.Common.GameObject(playerGameObject.Id, "Local Player", transform.Position.X, transform.Position.Y);
            return new EnterWorldOperationResponseParameters(gameObject);
        }

        private IGameObject CreatePlayerGameObject()
        {
            var scene = sceneContainer.GetScene(Maps.Map_1);
            return scene.AddGameObject(new InterestManagement.GameObject("Player", scene, new Vector2(18, -5.5f), new Vector2(10, 5)));
        }
    }
}