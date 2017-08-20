using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhotonCommonImplementation;
using UnityEngine;

namespace PhotonClientImplementation
{
    public sealed class PhotonPeer : IServerPeer, IPhotonPeerListener, IOperationRequestSender, IEventNotifier, IPeerDisconnectionNotifier, IOperationResponseNotifier
    {
        public ExitGames.Client.Photon.PhotonPeer RawPeer { get; }

        public bool IsConnected => RawPeer.PeerState == PeerStateValue.Connected;

        public NetworkTrafficState NetworkTrafficState
        {
            get
            {
                return networkTrafficState;
            }
            set
            {
                if (networkTrafficState == NetworkTrafficState.Paused && value == NetworkTrafficState.Flowing)
                {
                    while (optionsBuffer.Count > 0)
                    {
                        switch (optionsBuffer.Dequeue())
                        {
                            case BufferOption.OperationResponse:
                                {
                                    var tuple = operationResponsesBuffer.Dequeue();
                                    var operationResponded = OperationResponded;

                                    if (operationResponded != null)
                                    {
                                        var messageResponseData = tuple.Item1;
                                        var num = (int)tuple.Item2;

                                        operationResponded(messageResponseData, (short)num);
                                    }
                                    break;
                                }
                            case BufferOption.Event:
                                {
                                    var rawMessageData1 = eventsBuffer.Dequeue();
                                    var eventRecieved = EventRecieved;

                                    if (eventRecieved != null)
                                    {
                                        var rawMessageData2 = rawMessageData1;
                                        eventRecieved(rawMessageData2);
                                    }
                                    break;
                                }
                            default:
                                {
                                    throw new ArgumentOutOfRangeException();
                                }
                        }
                    }

                    Debug.Assert(operationResponsesBuffer.Count == 0, "Buffer has more than what flushed");
                    Debug.Assert(eventsBuffer.Count == 0, "Buffer has more than what flushed");
                }

                networkTrafficState = value;
            }
        }

        public PeerStateValue State => RawPeer.PeerState;

        public IPeerDisconnectionNotifier PeerDisconnectionNotifier => this;
        public IOperationRequestSender OperationRequestSender => this;
        public IOperationResponseNotifier OperationResponseNotifier => this;
        public IEventNotifier EventNotifier => this;

        public event Action<StatusCode> StatusChanged;
        public event Action<DisconnectReason, string> Disconnected;

        public event Action<RawMessageData> EventRecieved;
        public event Action<RawMessageResponseData, short> OperationResponded;

        private readonly ICoroutinesExecuter coroutinesExecuter;
        private readonly PeerConnectionInformation serverConnectionInformation;

        private short requestId;

        private readonly Queue<RawMessageData> eventsBuffer;
        private readonly Queue<Tuple<RawMessageResponseData, short>> operationResponsesBuffer;
        private readonly Queue<BufferOption> optionsBuffer;

        private NetworkTrafficState networkTrafficState;

        public PhotonPeer(PeerConnectionInformation serverConnectionInformation,
            ConnectionProtocol connectionProtocol, DebugLevel debugLevel, ICoroutinesExecuter coroutinesExecuter)
        {
            NetworkTrafficState = NetworkTrafficState.Flowing;

            this.serverConnectionInformation = serverConnectionInformation;

            RawPeer = new ExitGames.Client.Photon.PhotonPeer(this, connectionProtocol) { ChannelCount = 4 };

            #if UNITY_WEBGL || UNITY_XBOXONE || WEBSOCKET
            if (connectionProtocol == ConnectionProtocol.WebSocket ||
                connectionProtocol == ConnectionProtocol.WebSocketSecure)
            {
                var websocketType = typeof(SocketWebTcpCoroutine);
                RawPeer.SocketImplementationConfig[ConnectionProtocol.WebSocket] = websocketType;
            }
            #endif 

            RawPeer.DebugOut = debugLevel;

            this.coroutinesExecuter = coroutinesExecuter;
            coroutinesExecuter.StartCoroutine(UpdateEngine());

            eventsBuffer = new Queue<RawMessageData>(10);
            operationResponsesBuffer = new Queue<Tuple<RawMessageResponseData, short>>(10);
            optionsBuffer = new Queue<BufferOption>(10);
        }

        private IEnumerator<IYieldInstruction> UpdateEngine()
        {
            while (true)
            {
                Update();
                yield return null;
            }
        }

        private void Update()
        {
            do
            {
                // Left blank intentionally
            } while (RawPeer.DispatchIncomingCommands());

            do
            {
                // Left blank intentionally
            } while (RawPeer.SendOutgoingCommands());
        }

        public void SetNetworkTrafficState(NetworkTrafficState state)
        {
            NetworkTrafficState = state;
        }

        public void Connect()
        {
            var serverAddress = $"{serverConnectionInformation.Ip}:{serverConnectionInformation.Port}";

            Debug.Assert(RawPeer.Connect(serverAddress, "Game"),
                $"PhotonPeer::Connect() -> Could not begin connection with: {serverAddress}");
        }

        public void Disconnect()
        {
            if (RawPeer.PeerState == PeerStateValue.Disconnected &&
                RawPeer.PeerState == PeerStateValue.Disconnecting)
            {
                return;
            }

            RawPeer.Disconnect();

            var photonPeer = this;
            coroutinesExecuter.WaitAndDo(photonPeer.WaitForDisconnect().StartCoroutine(coroutinesExecuter), Dispose);

            Disconnected?.Invoke(DisconnectReason.ClientDisconnect, null);
        }

        public void DebugReturn(DebugLevel level, string message)
        {
            switch (level)
            {
                case DebugLevel.ERROR:
                {
                    Debug.LogError($"PhotonPeer::DebugReturn() -> Debug Level: {level} Message: {message}");
                    break;
                }
                case DebugLevel.WARNING:
                {
                    Debug.LogWarning($"PhotonPeer::DebugReturn() -> Debug Level: {level} Message: {message}");
                    break;
                }
                case DebugLevel.INFO:
                {
                    Debug.Log($"PhotonPeer::DebugReturn() -> Debug Level: {level} Message: {message}");
                    break;
                }
                default:
                {
                    Debug.Log($"PhotonPeer::DebugReturn() -> Debug Level: {level} Message: {message}");
                    break;
                }
            }
        }

        public void OnOperationResponse(OperationResponse operationResponse)
        {
            var requestId = operationResponse.ExtractRequestId();
            var parameter = operationResponse.Parameters[0] as byte[];

            var messageResponseData = new RawMessageResponseData(operationResponse.OperationCode, parameter, operationResponse.ReturnCode);

            if (NetworkTrafficState == NetworkTrafficState.Paused)
            {
                optionsBuffer.Enqueue(PhotonPeer.BufferOption.OperationResponse);
                operationResponsesBuffer.Enqueue(new Tuple<RawMessageResponseData, short>(messageResponseData, requestId));
            }
            else
            {
                var operationResponded = OperationResponded;
                if (operationResponded == null)
                {
                    return;
                }

                var numberId = (int)requestId;
                operationResponded(messageResponseData, (short)numberId);
            }
        }

        public void OnStatusChanged(StatusCode statusCode)
        {
            var disconnectReason = 0;

            switch (statusCode)
            {
                case StatusCode.Disconnect:
                    {
                        // Left blank intentionally
                        break;
                    }
                case StatusCode.TimeoutDisconnect:
                    {
                        disconnectReason = 1;
                        break;
                    }
                case StatusCode.DisconnectByServer:
                case StatusCode.DisconnectByServerUserLimit:
                case StatusCode.DisconnectByServerLogic:
                    {
                        disconnectReason = 4;
                        break;
                    }
            }

            Disconnected?.Invoke((DisconnectReason)disconnectReason, statusCode.ToString());
            StatusChanged?.Invoke(statusCode);
        }

        public void OnEvent(EventData eventData)
        {
            var rawMessageData = new RawMessageData(eventData.Code, eventData.Parameters[0] as byte[]);

            if (NetworkTrafficState == NetworkTrafficState.Paused)
            {
                optionsBuffer.Enqueue(BufferOption.Event);
                eventsBuffer.Enqueue(rawMessageData);
            }
            else
            {
                EventRecieved?.Invoke(rawMessageData);
            }
        }

        public void Dispose()
        {
            coroutinesExecuter?.Dispose();
        }

        public short Send<TParam>(MessageData<TParam> data, MessageSendOptions sendOptions)
            where TParam : IParameters, new()
        {
            if (sendOptions.Flush)
            {
                Debug.Log("PhotonPeer::Send() -> SendOptions::Flush is not supported!");
            }

            requestId = (short)(requestId + 1);

            Debug.Assert(RawPeer.OpCustom(
                Utils.ToPhotonOperationRequest(data, requestId), sendOptions.Reliable, sendOptions.ChannelId, sendOptions.Encrypted),
                "PhotonPeer::Send() -> Could not send operation request!");

            return requestId;
        }

        private enum BufferOption
        {
            OperationResponse,
            Event
        }
    }

    public static class PeerUtils
    {
        public static IEnumerator<IYieldInstruction> WaitForDisconnect(this PhotonPeer peer)
        {
            do
            {
                // Left blank intentionally
                yield return null;
            }
            while ((uint)peer.State > 0U);
        }

        public static IEnumerator<IYieldInstruction> WaitForConnect(this PhotonPeer peer, Action onConnected, Action onConnectionFailed)
        {
            do
            {
                // Left blank intentionally
                yield return null;
            }
            while (peer.State == PeerStateValue.Connecting || peer.State == PeerStateValue.InitializingApplication);

            switch (peer.State)
            {
                case PeerStateValue.Connected:
                    {
                        onConnected?.Invoke();
                        break;
                    }
                case PeerStateValue.Disconnected:
                    {
                        onConnectionFailed?.Invoke();
                        break;
                    }
                default:
                    {
                        Debug.LogWarning($"PeerUtils::WaitForConnect() -> Unexpected state while waiting for connection: {peer.State}");
                        break;
                    }
            }
        }
    }

    public static class Utils
    {
        public static OperationRequest ToPhotonOperationRequest<TParam>(MessageData<TParam> request, short requestId) where TParam : IParameters, new()
        {
            var photonParameters = PhotonCommonImplementation.Utils.ToPhotonParameters(request.Parameters);
            var operationRequest = new OperationRequest
            {
                OperationCode = request.Code,
                Parameters = photonParameters
            };

            operationRequest.SetRequestId(requestId);

            return operationRequest;
        }

        public static OperationResponse ToPhotonOperationResponse(RawMessageResponseData response, short requestId)
        {
            var photonParameters = PhotonCommonImplementation.Utils.ToPhotonParameters(response.Parameters);
            var operationResponse = new OperationResponse
            {
                OperationCode = response.Code,
                Parameters = photonParameters,
                ReturnCode = response.ReturnCode
            };

            operationResponse.SetRequestId(requestId);

            return operationResponse;
        }

        public static void SetRequestId(this OperationRequest operationRequest, short requestId)
        {
            if (operationRequest.Parameters == null)
            {
                operationRequest.Parameters = new Dictionary<byte, object>();
            }

            PhotonCommonImplementation.Utils.SetRequestId(operationRequest.Parameters, requestId);
        }

        public static void SetRequestId(this OperationResponse operationResponse, short requestId)
        {
            if (operationResponse.Parameters == null)
            {
                operationResponse.Parameters = new Dictionary<byte, object>();
            }

            PhotonCommonImplementation.Utils.SetRequestId(operationResponse.Parameters, requestId);
        }

        public static short ExtractRequestId(this OperationRequest operationRequest)
        {
            return PhotonCommonImplementation.Utils.ExtractRequestId(operationRequest.Parameters);
        }

        public static short ExtractRequestId(this OperationResponse operationResponse)
        {
            return ExtractRequestId(operationResponse.Parameters);
        }

        private static short ExtractRequestId(IReadOnlyDictionary<byte, object> parameters)
        {
            var parameterCodeValue = GetAdditionalParameterCodeValue(AdditionalParameterCode.RequestId);

            Debug.Assert(parameters.ContainsKey(parameterCodeValue), "ExtractRequestId() -> Could not find requestId");

            return (short)parameters[parameterCodeValue];
        }

        private static byte GetAdditionalParameterCodeValue(AdditionalParameterCode parameterCode)
        {
            return (byte)(byte.MaxValue - parameterCode);
        }
    }

    public class PhotonServerConnector
    {
        private readonly Func<ICoroutinesExecuter> coroutinesExecuterProvider;

        public PhotonServerConnector(Func<ICoroutinesExecuter> coroutinesExecuterProvider)
        {
            this.coroutinesExecuterProvider = coroutinesExecuterProvider;
        }

        public async Task<IServerPeer> ConnectAsync(Yield yield, PeerConnectionInformation connectionInformation, ConnectionDetails connectionDetails)
        {
            var coroutinesExecuter = coroutinesExecuterProvider.Invoke();

            var photonPeer = new PhotonPeer(connectionInformation, connectionDetails.ConnectionProtocol, connectionDetails.DebugLevel, coroutinesExecuter);
            photonPeer.Connect();

            var statusCode = await WaitForStatusCodeChange(yield, photonPeer);
            if (statusCode == StatusCode.Connect)
            {

                Debug.Log($"A new server has been connected: {connectionInformation.Ip}:{connectionInformation.Port}");
                return photonPeer;
            }

            Debug.LogError($"Connecting to {connectionInformation.Ip}:{connectionInformation.Port} failed. StatusCode: {statusCode}");
            return null;
        }

        private async Task<StatusCode> WaitForStatusCodeChange(Yield yield, PhotonPeer photonPeer)
        {
            var statusChanged = false;
            var statusCode = StatusCode.Disconnect;

            Action<StatusCode> onStatusChanged = (newStatusCode) =>
            {
                statusChanged = true;
                statusCode = newStatusCode;
            };

            photonPeer.StatusChanged += onStatusChanged;

            await yield.Return(new WaitUntilIsTrue(() => statusChanged));

            photonPeer.StatusChanged -= onStatusChanged;

            return statusCode;

        }
    }

    public struct ConnectionDetails
    {
        public ConnectionProtocol ConnectionProtocol { get; }
        public DebugLevel DebugLevel { get; }

        public ConnectionDetails(ConnectionProtocol connectionProtocol, DebugLevel debugLevel)
        {
            ConnectionProtocol = connectionProtocol;
            DebugLevel = debugLevel;
        }
    }
}