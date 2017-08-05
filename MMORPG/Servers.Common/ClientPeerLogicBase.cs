using System;
using System.Collections.Generic;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace Shared.Servers.Common
{
    public abstract class ClientPeerLogicBase : ClientPeer
    {
        private readonly Dictionary<byte, IOperation> operationsContainer = new Dictionary<byte, IOperation>();

        protected ClientPeerLogicBase(InitRequest initRequest)
            : base(initRequest)
        {
            EventSender.EventAction = (eventCode, parameters, options) => SendEvent(new EventData(eventCode, parameters), options);
        }

        protected void SetOperationRequestHandler<T>(T operationCode, IOperation operation)
            where T : struct, IComparable, IConvertible, IFormattable
        {
            operationsContainer.Add(Convert.ToByte(operationCode), operation);
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            operationsContainer.Clear();

            OnPeerDisconnect(reasonCode, reasonDetail);
        }

        protected abstract void OnPeerDisconnect(DisconnectReason reasonCode, string reasonDetail);

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            var response = OperationRequestHandler(operationRequest);
            SendOperationResponse(response, sendParameters);
        }

        private OperationResponse OperationRequestHandler(OperationRequest operationRequest)
        {
            var operationCode = operationRequest.OperationCode;

            var operationHandler = operationsContainer.TryGetValue(operationRequest.OperationCode, out var operation);
            if (!operationHandler)
            {
                return new OperationResponse { ReturnCode = 1, DebugMessage = "Invalid operation requested!" };
            }

            var responseParameters = operation.OperationHandler(operationRequest.Parameters);
            return responseParameters == null
                ? new OperationResponse(operationCode)
                : new OperationResponse(operationCode, responseParameters);
        }
    }
}