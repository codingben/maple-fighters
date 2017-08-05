using System.Collections.Generic;
using log4net.Repository.Hierarchy;
using Photon.SocketServer;
using Photon.SocketServer.Rpc;
using PhotonHostRuntimeInterfaces;

namespace Shared.Servers.Common
{
    internal abstract class ClientPeerLogicBase : ClientPeer
    {
        private readonly Dictionary<byte, IOperationHandler> operationsContainer = new Dictionary<byte, IOperationHandler>();

        protected ClientPeerLogicBase(InitRequest initRequest)
            : base(initRequest)
        {
            EventSender.EventAction = (eventCode, parameters, options) => SendEvent(new EventData(eventCode, parameters), options);
        }

        protected void SetOperationRequestHandler(GameOperations operationCode, IOperationHandler operation)
        {
            operationsContainer.Add((byte)operationCode, operation);
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            Logger.Log.Debug($"A peer disconnected - {RemoteIP}:{RemotePort}");

            operationsContainer.Clear();
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            var response = OperationRequestHandler(operationRequest);
            SendOperationResponse(response, sendParameters);
        }

        public OperationResponse OperationRequestHandler(OperationRequest operationRequest)
        {
            var operationCode = operationRequest.OperationCode;

            var operationHandler = operationsContainer.TryGetValue(operationRequest.OperationCode, out var operation);
            if (!operationHandler)
            {
                return new OperationResponse { ReturnCode = 1, DebugMessage = "Invalid operation requested!" };
            }

            var responseParameters = operation.Handle(operationRequest.Parameters);
            return responseParameters == null
                ? new OperationResponse(operationCode)
                : new OperationResponse(operationCode, responseParameters);
        }
    }
}