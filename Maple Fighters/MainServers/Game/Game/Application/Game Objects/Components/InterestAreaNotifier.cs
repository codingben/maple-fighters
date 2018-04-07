using System.Collections.Generic;
using System.Linq;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using Game.Application.PeerLogic.Components.Interfaces;
using Game.Application.GameObjects.Components.Interfaces;
using InterestManagement.Components.Interfaces;
using PeerLogic.Common.Components.Interfaces;
using ServerApplication.Common.ApplicationBase;

namespace Game.Application.GameObjects.Components
{
    internal class InterestAreaNotifier : Component<ISceneObject>, IInterestAreaNotifier
    {
        private IPeerContainer peerContainer;

        protected override void OnAwake()
        {
            base.OnAwake();

            peerContainer = ServerComponents.GetComponent<IPeerContainer>().AssertNotNull();
        }

        public void NotifySubscribers<TParameters>(byte code, TParameters parameters, MessageSendOptions messageSendOptions)
            where TParameters : struct, IParameters
        {
            foreach (var subscriber in GetSubscribersFromPublishers)
            {
                var peerId = subscriber.Components.GetComponent<IPeerIdGetter>();
                if (peerId == null)
                {
                    continue;
                }

                var peerWrapper = peerContainer.GetPeerWrapper(peerId.GetId());
                if (peerWrapper == null)
                {
                    continue;
                }

                if (!peerWrapper.Peer.IsConnected)
                {
                    continue;
                }

                var eventSender = peerWrapper.PeerLogic.Components.GetComponent<IEventSenderWrapper>().AssertNotNull();
                eventSender.Send(code, parameters, messageSendOptions);
            }
        }

        public void NotifySubscriberOnly<TParameters>(int peerId, byte code, TParameters parameters, MessageSendOptions messageSendOptions)
            where TParameters : struct, IParameters
        {
            var peerWrapper = peerContainer.GetPeerWrapper(peerId);
            if (peerWrapper == null || !peerWrapper.Peer.IsConnected)
            {
                return;
            }

            var eventSender = peerWrapper.PeerLogic.Components.GetComponent<IEventSenderWrapper>().AssertNotNull();
            eventSender.Send(code, parameters, messageSendOptions);
        }

        private IEnumerable<ISceneObject> GetSubscribersFromPublishers
        {
            get
            {
                var subscribers = new List<ISceneObject>();

                var interestArea = Entity.Components.GetComponent<IInterestArea>().AssertNotNull();
                if (interestArea == null)
                {
                    return subscribers.ToArray();
                }

                foreach (var publisher in interestArea.GetSubscribedPublishers())
                {
                    subscribers.AddRange(publisher.GetAllSubscribers().Where(subscriber => subscriber.Id != Entity.Id));
                }
                return subscribers.ToArray();
            }
        }
    }
}