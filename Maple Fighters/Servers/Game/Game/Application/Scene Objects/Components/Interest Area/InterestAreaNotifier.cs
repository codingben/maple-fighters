using System.Collections.Generic;
using System.Linq;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using Game.Application.PeerLogic.Components;
using Game.InterestManagement;
using PeerLogic.Common.Components;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.Components;

namespace Game.Application.SceneObjects.Components
{
    internal class InterestAreaNotifier : Component<ISceneObject>, IInterestAreaNotifier
    {
        private IPeerContainer peerContainer;

        protected override void OnAwake()
        {
            base.OnAwake();

            peerContainer = Server.Entity.GetComponent<IPeerContainer>().AssertNotNull();
        }

        public void NotifySubscribers<TParameters>(byte code, TParameters parameters, MessageSendOptions messageSendOptions)
            where TParameters : struct, IParameters
        {
            foreach (var subscriber in GetSubscribersFromPublishers)
            {
                var peerId = subscriber.Container.GetComponent<IPeerIdGetter>();
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

                var eventSender = peerWrapper.PeerLogic.Entity.GetComponent<IEventSenderWrapper>().AssertNotNull();
                eventSender.Send(code, parameters, messageSendOptions);
            }
        }

        public void NotifySubscriberOnly<TParameters>(int subscriberId, byte code, TParameters parameters, MessageSendOptions messageSendOptions)
            where TParameters : struct, IParameters
        {
            var peerWrapper = peerContainer.GetPeerWrapper(subscriberId);
            if (peerWrapper == null)
            {
                return;
            }

            if (peerWrapper.Peer.IsConnected)
            {
                var eventSender = peerWrapper.PeerLogic.Entity.GetComponent<IEventSenderWrapper>().AssertNotNull();
                eventSender.Send(code, parameters, messageSendOptions);
            }
        }

        private IEnumerable<ISceneObject> GetSubscribersFromPublishers
        {
            get
            {
                var subscribers = new List<ISceneObject>();

                var interestArea = Entity.Container.GetComponent<IInterestArea>().AssertNotNull();
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