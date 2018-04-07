using System.Collections.Generic;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using Game.Application.PeerLogic.Components.Interfaces;
using Game.Common;
using InterestManagement.Components.Interfaces;
using PeerLogic.Common.Components.Interfaces;

namespace Game.Application.PeerLogic.Components
{
    internal class InterestManagementNotifier : Component
    {
        private IEventSenderWrapper eventSender;

        protected override void OnAwake()
        {
            base.OnAwake();

            eventSender = Components.GetComponent<IEventSenderWrapper>().AssertNotNull();

            SubscribeToInterestAreaEvents();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            UnsubscribeFromInterestAreaEvents();
        }

        private void SubscribeToInterestAreaEvents()
        {
            var playerGameObjectGetter = Components.GetComponent<IPlayerGameObjectGetter>().AssertNotNull();
            var interestArea = playerGameObjectGetter.GetPlayerGameObject().Components.GetComponent<IInterestAreaEvents>().AssertNotNull();
            interestArea.SubscriberAdded += OnSubscriberAdded;
            interestArea.SubscriberRemoved += OnSubscriberRemoved;
            interestArea.SubscribersAdded += OnSubscribersAdded;
            interestArea.SubscribersRemoved += OnSubscribersRemoved;
        }

        private void UnsubscribeFromInterestAreaEvents()
        {
            var playerGameObjectGetter = Components.GetComponent<IPlayerGameObjectGetter>().AssertNotNull();
            var interestArea = playerGameObjectGetter.GetPlayerGameObject().Components.GetComponent<IInterestAreaEvents>().AssertNotNull();
            interestArea.SubscriberAdded -= OnSubscriberAdded;
            interestArea.SubscriberRemoved -= OnSubscriberRemoved;
            interestArea.SubscribersAdded -= OnSubscribersAdded;
            interestArea.SubscribersRemoved -= OnSubscribersRemoved;
        }

        private void OnSubscriberAdded(ISceneObject sceneObject)
        {
            var positionTransform = sceneObject.Components.GetComponent<IPositionTransform>().AssertNotNull();
            var sharedSceneObject = new SceneObjectParameters(sceneObject.Id, sceneObject.Name, positionTransform.Position.X, positionTransform.Position.Y);

            var parameters = new SceneObjectAddedEventParameters(sharedSceneObject);
            eventSender.Send((byte)GameEvents.SceneObjectAdded, parameters, MessageSendOptions.DefaultReliable());
        }

        private void OnSubscriberRemoved(int subscriberId)
        {
            var parameters = new SceneObjectRemovedEventParameters(subscriberId);
            eventSender.Send((byte)GameEvents.SceneObjectRemoved, parameters, MessageSendOptions.DefaultReliable());
        }

        private void OnSubscribersAdded(IReadOnlyList<ISceneObject> sceneObjects)
        {
            var sharedSceneObjects = new SceneObjectParameters[sceneObjects.Count];
            for (var i = 0; i < sceneObjects.Count; i++)
            {
                sharedSceneObjects[i].Id = sceneObjects[i].Id;
                sharedSceneObjects[i].Name = sceneObjects[i].Name;

                var positionTransform = sceneObjects[i].Components.GetComponent<IPositionTransform>().AssertNotNull();
                sharedSceneObjects[i].X = positionTransform.Position.X;
                sharedSceneObjects[i].Y = positionTransform.Position.Y;
            }

            var parameters = new SceneObjectsAddedEventParameters(sharedSceneObjects);
            eventSender.Send((byte)GameEvents.SceneObjectsAdded, parameters, MessageSendOptions.DefaultReliable());
        }

        private void OnSubscribersRemoved(int[] sceneObjectsId)
        {
            var parameters = new SceneObjectsRemovedEventParameters(sceneObjectsId);
            eventSender.Send((byte)GameEvents.SceneObjectsRemoved, parameters, MessageSendOptions.DefaultReliable());
        }
    }
}