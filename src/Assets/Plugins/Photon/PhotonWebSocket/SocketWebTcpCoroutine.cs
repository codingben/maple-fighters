#if UNITY_WEBGL || UNITY_XBOXONE || WEBSOCKET
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SocketTcp.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Internal class to encapsulate the network i/o functionality for the realtime libary.
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using UnityEngine;
using SupportClassPun = ExitGames.Client.Photon.SupportClass;


namespace ExitGames.Client.Photon
{
    #if UNITY_5_3 || UNITY_5_3_OR_NEWER
    /// <summary>
    /// Yield Instruction to Wait for real seconds. Very important to keep connection working if Time.TimeScale is altered, we still want accurate network events
    /// </summary>
    public sealed class WaitForRealSeconds : CustomYieldInstruction
    {
        private readonly float _endTime;

        public override bool keepWaiting
        {
            get { return _endTime > Time.realtimeSinceStartup; }
        }

        public WaitForRealSeconds(float seconds)
        {
            _endTime = Time.realtimeSinceStartup + seconds;
        }
    }
    #endif

    /// <summary>
    /// Internal class to encapsulate the network i/o functionality for the realtime libary.
    /// </summary>
    public class SocketWebTcpCoroutine : IPhotonSocket, IDisposable
    {
        private WebSocket sock;

        private readonly object syncer = new object();

        public SocketWebTcpCoroutine(PeerBase npeer) : base(npeer)
        {
            if (this.ReportDebugOfLevel(DebugLevel.INFO))
            {
                Listener.DebugReturn(DebugLevel.INFO, "new SocketWebTcpCoroutine(). Server: " + this.ConnectAddress + " protocol: " + this.Protocol);
            }

            switch (this.Protocol)
            {
                case ConnectionProtocol.WebSocket:
                    break;
                case ConnectionProtocol.WebSocketSecure:
                    break;
                default:
                    throw new Exception("Protocol '" + this.Protocol + "' not supported by WebSocket");
            }

            this.PollReceive = false;
        }

        public void Dispose()
        {
            this.State = PhotonSocketState.Disconnecting;

            if (this.sock != null)
            {
                try
                {
                    if (this.sock.Connected) this.sock.Close();
                }
                catch (Exception ex)
                {
                    this.EnqueueDebugReturn(DebugLevel.INFO, "Exception in Dispose(): " + ex);
                }
            }

            this.sock = null;
            this.State = PhotonSocketState.Disconnected;
        }

        GameObject websocketConnectionObject;
        public override bool Connect()
        {
            bool baseOk = base.Connect();
            if (!baseOk)
            {
                return false;
            }

            this.State = PhotonSocketState.Connecting;

            if (this.websocketConnectionObject != null)
            {
                UnityEngine.Object.Destroy(this.websocketConnectionObject);
            }

            this.websocketConnectionObject = new GameObject("websocketConnectionObject");
            MonoBehaviour mb = this.websocketConnectionObject.AddComponent<MonoBehaviourExt>();
            this.websocketConnectionObject.hideFlags = HideFlags.HideInHierarchy;
            UnityEngine.Object.DontDestroyOnLoad(this.websocketConnectionObject);

            this.sock = new WebSocket(new Uri(this.ConnectAddress));

            mb.StartCoroutine(this.ReceiveLoop());
            return true;
        }


        public override bool Disconnect()
        {
            if (this.ReportDebugOfLevel(DebugLevel.INFO))
            {
                this.Listener.DebugReturn(DebugLevel.INFO, "SocketWebTcpCoroutine.Disconnect()");
            }

            this.State = PhotonSocketState.Disconnecting;

            lock (this.syncer)
            {
                if (this.sock != null)
                {
                    try
                    {
                        this.sock.Close();
                    }
                    catch (Exception ex)
                    {
                        this.Listener.DebugReturn(DebugLevel.ERROR, "Exception in Disconnect(): " + ex);
                    }
                    this.sock = null;
                }
            }

            if (this.websocketConnectionObject != null)
            {
                UnityEngine.Object.Destroy(this.websocketConnectionObject);
            }

            this.State = PhotonSocketState.Disconnected;
            return true;
        }

        /// <summary>
        /// used by TPeer*
        /// </summary>
        public override PhotonSocketError Send(byte[] data, int length)
        {
            if (this.State != PhotonSocketState.Connected)
            {
                return PhotonSocketError.Skipped;
            }

            try
            {
                if (this.ReportDebugOfLevel(DebugLevel.ALL))
                {
                    this.Listener.DebugReturn(DebugLevel.ALL, "Sending: " + SupportClassPun.ByteArrayToString(data));
                }

                if (this.sock != null)
                {
                    this.sock.Send(data);
                }
            }
            catch (Exception e)
            {
                this.Listener.DebugReturn(DebugLevel.ERROR, "Cannot send to: " + this.ConnectAddress + ". " + e.Message);

                this.HandleException(StatusCode.Exception);
                return PhotonSocketError.Exception;
            }

            return PhotonSocketError.Success;
        }

        public override PhotonSocketError Receive(out byte[] data)
        {
            data = null;
            return PhotonSocketError.NoData;
        }


        internal const int ALL_HEADER_BYTES = 9;
        internal const int TCP_HEADER_BYTES = 7;
        internal const int MSG_HEADER_BYTES = 2;

        public IEnumerator ReceiveLoop()
        {
            if (this.sock != null)
            {
                this.sock.Connect();

                while (this.sock != null && !this.sock.Connected && this.sock.Error == null)
                {
                    #if UNITY_5_3 || UNITY_5_3_OR_NEWER
                    yield return new WaitForRealSeconds(0.02f);
                    #else
                    float waittime = Time.realtimeSinceStartup + 0.2f;
                    while (Time.realtimeSinceStartup < waittime) yield return 0;
                    #endif
                }

                if (this.sock != null)
                {
                    if (this.sock.Error != null)
                    {
                        this.Listener.DebugReturn(DebugLevel.ERROR, "Exiting receive thread. Server: " + this.ConnectAddress + " Error: " + this.sock.Error);
                        this.HandleException(StatusCode.ExceptionOnConnect);
                    }
                    else
                    {
                        // connected
                        this.State = PhotonSocketState.Connected;
                        this.peerBase.OnConnect();

                        while (this.State == PhotonSocketState.Connected)
                        {
                            if (this.sock != null)
                            {
                                if (this.sock.Error != null)
                                {
                                    this.Listener.DebugReturn(DebugLevel.ERROR, "Exiting receive thread (inside loop). Server: " + this.ConnectAddress + " Error: " + this.sock.Error);
                                    this.HandleException(StatusCode.ExceptionOnReceive);
                                    break;
                                }
                                else
                                {
                                    byte[] inBuff = this.sock.Recv();
                                    if (inBuff == null || inBuff.Length == 0)
                                    {
                                        // nothing received. wait a bit, try again
                                        #if UNITY_5_3 || UNITY_5_3_OR_NEWER
                                        yield return new WaitForRealSeconds(0.02f);
                                        #else
                                        float waittime = Time.realtimeSinceStartup + 0.02f;
                                        while (Time.realtimeSinceStartup < waittime) yield return 0;
                                        #endif
                                        continue;
                                    }

                                    if (this.ReportDebugOfLevel(DebugLevel.ALL))
                                    {
                                        this.Listener.DebugReturn(DebugLevel.ALL, "TCP << " + inBuff.Length + " = " + SupportClassPun.ByteArrayToString(inBuff));
                                    }

                                    if (inBuff.Length > 0)
                                    {
                                        try
                                        {
                                            HandleReceivedDatagram(inBuff, inBuff.Length, false);
                                        }
                                        catch (Exception e)
                                        {
                                            if (this.State != PhotonSocketState.Disconnecting && this.State != PhotonSocketState.Disconnected)
                                            {
                                                if (this.ReportDebugOfLevel(DebugLevel.ERROR))
                                                {
                                                    this.EnqueueDebugReturn(DebugLevel.ERROR, "Receive issue. State: " + this.State + ". Server: '" + this.ConnectAddress + "' Exception: " + e);
                                                }

                                                this.HandleException(StatusCode.ExceptionOnReceive);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            this.Disconnect();
        }
    }

    internal class MonoBehaviourExt : MonoBehaviour { }
}

#endif
