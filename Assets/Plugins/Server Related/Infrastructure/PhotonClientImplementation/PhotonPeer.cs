using System;
using System.Collections.Generic;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using ExitGames.Client.Photon;

namespace PhotonClientImplementation
{
    public sealed class PhotonPeer : IServerPeer, IPhotonPeerListener, IOperationRequestSender, IEventNotifier, IPeerDisconnectionNotifier, IOperationResponseNotifier
    {
        public ExitGames.Client.Photon.PhotonPeer RawPeer { get; }

        public bool IsConnected => RawPeer?.PeerState == PeerStateValue.Connected;

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
                        var op = optionsBuffer.Dequeue();
                        switch (op)
                        {
                            case BufferOption.OperationResponse:
                            {
                                var opres = operationResponsesBuffer.Dequeue();
                                OperationResponded?.Invoke(opres.Item1, opres.Item2);
                                break;
                            }
                            case BufferOption.Event:
                            {
                                var ev = eventsBuffer.Dequeue();
                                EventRecieved?.Invoke(ev);
                                break;
                            }
                            default:
                            {
                                throw new ArgumentOutOfRangeException();
                            }
                        }
                    }

                    LogUtils.Assert(operationResponsesBuffer.Count == 0, new TraceMessage("Buffer has more than what flushed"));
                    LogUtils.Assert(eventsBuffer.Count == 0, new TraceMessage("Buffer has more than what flushed"));
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

        private readonly PeerConnectionInformation serverConnectionInformation;

        private short requestId;

        private readonly Queue<RawMessageData> eventsBuffer;
        private readonly Queue<Tuple<RawMessageResponseData, short>> operationResponsesBuffer;
        private readonly Queue<BufferOption> optionsBuffer;

        private NetworkTrafficState networkTrafficState;

        public PhotonPeer()
        {
            // Left blank intentionally
        }

        public PhotonPeer(PeerConnectionInformation serverConnectionInformation,
            ConnectionProtocol connectionProtocol, DebugLevel debugLevel, ICoroutinesExecutor coroutinesExecuter)
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
            const string APPLICATION_NAME = "Empty";

            var serverAddress = serverConnectionInformation.Ip + ":" + serverConnectionInformation.Port;
            var isConnecting = RawPeer.Connect(serverAddress, APPLICATION_NAME);

            LogUtils.Assert(isConnecting, new TraceMessage("Could not begin connection with: " + serverAddress + " <" + APPLICATION_NAME + ">"));
        }

        public void Disconnect()
        {
            if (RawPeer.PeerState == PeerStateValue.Disconnected ||
                RawPeer.PeerState == PeerStateValue.Disconnecting)
            {
                return;
            }

            RawPeer.Disconnect();
            RawPeer.StopThread();

            Disconnected?.Invoke(DisconnectReason.ClientDisconnect, null);
        }

        public void DebugReturn(DebugLevel level, string message)
        {
            switch (level)
            {
                case DebugLevel.ERROR:
                {
                    LogUtils.Log(MessageBuilder.Trace($"Debug Level: {level} Message: {message}"), LogMessageType.Error);
                    break;
                }
                case DebugLevel.WARNING:
                {
                    LogUtils.Log(MessageBuilder.Trace($"Debug Level: {level} Message: {message}"), LogMessageType.Warning);
                    break;
                }
                case DebugLevel.INFO:
                {
                    LogUtils.Log(MessageBuilder.Trace($"Debug Level: {level} Message: {message}"));
                    break;
                }
                default:
                {
                    LogUtils.Log(MessageBuilder.Trace($"Debug Level: {level} Message: {message}"));
                    break;
                }
            }
        }

        public void OnOperationResponse(OperationResponse operationResponse)
        {
            var requestId = operationResponse.ExtractRequestId();
            var binaryParameters = operationResponse.Parameters[0] as byte[];
            var messageData = new RawMessageResponseData(operationResponse.OperationCode, binaryParameters, operationResponse.ReturnCode);

            if (NetworkTrafficState == NetworkTrafficState.Paused)
            {
                optionsBuffer.Enqueue(BufferOption.OperationResponse);
                operationResponsesBuffer.Enqueue(new Tuple<RawMessageResponseData, short>(messageData, requestId));
                return;
            }

            OperationResponded?.Invoke(messageData, requestId);
        }

        public void OnStatusChanged(StatusCode statusCode)
        {
            switch (statusCode)
            {
                case StatusCode.Disconnect:
                {
                    Disconnected?.Invoke(DisconnectReason.ManagedDisconnect, null);
                    break;
                }
                case StatusCode.TimeoutDisconnect:
                {
                    Disconnected?.Invoke(DisconnectReason.TimeoutDisconnect, null);
                    break;
                }
                case StatusCode.DisconnectByServer:
                case StatusCode.DisconnectByServerUserLimit:
                case StatusCode.DisconnectByServerLogic:
                {
                    Disconnected?.Invoke(DisconnectReason.ServerDisconnect, statusCode.ToString());
                    break;
                }
            }

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

        public short Send<TParam>(MessageData<TParam> data, MessageSendOptions sendOptions)
            where TParam : struct, IParameters
        {
            if (sendOptions.Flush)
            {
                LogUtils.Log(MessageBuilder.Trace("sendOptions::Flush is not supported!"), LogMessageType.Warning);
            }

            var currentRequestId = requestId++;
            var operationRequest = Utils.ToPhotonOperationRequest(data, currentRequestId);

            var result = RawPeer.OpCustom(operationRequest, sendOptions.Reliable, sendOptions.ChannelId, sendOptions.Encrypted);
            LogUtils.Assert(result, "Could not send operation request");
            return currentRequestId;
        }

        private enum BufferOption
        {
            OperationResponse,
            Event
        }
    }
}