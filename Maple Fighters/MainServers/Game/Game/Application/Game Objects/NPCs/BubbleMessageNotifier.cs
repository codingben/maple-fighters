using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using Game.Application.GameObjects.Components.Interfaces;
using Game.Application.PeerLogic.Components.Interfaces;
using Game.Common;
using InterestManagement.Components.Interfaces;

namespace Game.Application.GameObjects
{
    public class BubbleMessageNotifier : Component<ISceneObject>
    {
        private readonly string message;
        private readonly int time;

        public BubbleMessageNotifier(string message, int time)
        {
            this.message = message;
            this.time = time;
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            var interestAreaEvents = Entity.Components.GetComponent<IInterestAreaEvents>().AssertNotNull();
            interestAreaEvents.SubscriberAdded += OnSubscriberAdded;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            var interestAreaEvents = Entity.Components.GetComponent<IInterestAreaEvents>().AssertNotNull();
            interestAreaEvents.SubscriberAdded -= OnSubscriberAdded;
        }

        private void OnSubscriberAdded(ISceneObject sceneObject)
        {
            var peerIdGetter = sceneObject.Components.GetComponent<IPeerIdGetter>();
            if (peerIdGetter != null)
            {
                RaiseBubbleMessage();
            }

            void RaiseBubbleMessage()
            {
                var interestAreaNotifier = Entity.Components.GetComponent<IInterestAreaNotifier>().AssertNotNull();
                var parameters = new BubbleMessageEventParameters(Entity.Id, message, time);
                interestAreaNotifier.NotifySubscriberOnly(peerIdGetter.GetId(), (byte)GameEvents.BubbleMessage, parameters, MessageSendOptions.DefaultReliable());
            }
        }
    }
}