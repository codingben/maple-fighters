﻿using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;
using UserProfile.Server.Common;
using UserProfile.Service.Application.Components.Interfaces;

namespace UserProfile.Service.Application.PeerLogic.Operations
{
    internal class RegisterToUserProfileServiceOperationHandler : IOperationRequestHandler<RegisterToUserProfileServiceRequestParameters, EmptyParameters>
    {
        private readonly int peerId;
        private readonly IServerIdToPeerIdConverter serverIdToPeerIdConverter;

        public RegisterToUserProfileServiceOperationHandler(int peerId)
        {
            this.peerId = peerId;

            serverIdToPeerIdConverter = ServerComponents.GetComponent<IServerIdToPeerIdConverter>().AssertNotNull();
        }

        public EmptyParameters? Handle(MessageData<RegisterToUserProfileServiceRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var serverId = messageData.Parameters.ServerId;
            serverIdToPeerIdConverter.Add(serverId, peerId);
            return null;
        }
    }
}