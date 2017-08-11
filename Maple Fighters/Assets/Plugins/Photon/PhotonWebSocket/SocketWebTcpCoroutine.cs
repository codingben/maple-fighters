// #if UNITY_WEBGL || UNITY_XBOXONE || WEBSOCKET
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SocketWebTcpCoroutine.cs" company="Exit Games GmbH">
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
using WebSocketSharp;
using SupportClassPun = ExitGames.Client.Photon.SupportClass;


namespace ExitGames.Client.Photon
{
    // #if UNITY_5_3 || UNITY_5_3_OR_NEWER
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
    //#endif

    /// <summary>
    /// Internal class to encapsulate the network i/o functionality for the realtime libary.
    /// </summary>
    public class SocketWebTcpCoroutine : IPhotonSocket, IDisposable
    {
        private WebSocket sock;

        private GameObject websocketConnectionObject;

        /// <summary>Constructor. Checks if "expected" protocol matches.</summary>
        public SocketWebTcpCoroutine(PeerBase npeer) 
            : base(npeer)
        {
            if (this.ReportDebugOfLevel(DebugLevel.INFO))
            {
                this.Listener.DebugReturn(DebugLevel.INFO, "new SocketWebTcpCoroutine(). Server: " + ServerAddress + " protocol: " + this.Protocol);
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

        /// <summary>Connect the websocket (base checks if this was already connected).</summary>
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

            sock = new WebSocket(ServerAddress);
            // connecting the socket is off-loaded into the coroutine which we start now

            mb.StartCoroutine(this.ReceiveLoop());
            return true;
        }


        /// <summary>Disconnect the websocket (no matter what it does right now).</summary>
        public override bool Disconnect()
        {
            if (this.State == PhotonSocketState.Disconnecting || this.State == PhotonSocketState.Disconnected)
            {
                return false;
            }

            if (this.ReportDebugOfLevel(DebugLevel.INFO))
            {
                this.Listener.DebugReturn(DebugLevel.INFO, "SocketWebTcpCoroutine.Disconnect()");
            }

            this.State = PhotonSocketState.Disconnecting;
            if (this.sock != null)
            {
                try
                {
                    this.sock.Close();
                }
                catch
                {
                }
                this.sock = null;
            }

            if (this.websocketConnectionObject != null)
            {
                UnityEngine.Object.Destroy(this.websocketConnectionObject);
            }

            this.State = PhotonSocketState.Disconnected;
            return true;
        }

        /// <summary>Calls Disconnect.</summary>
        public void Dispose()
        {
            this.Disconnect();
        }


        /// <summary>Used by TPeer to send.</summary>
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

                this.sock.Send(data);
            }
            catch (Exception e)
            {
                this.Listener.DebugReturn(DebugLevel.ERROR, "Cannot send to: " + ServerAddress + ". " + e.Message);

                if (this.State == PhotonSocketState.Connected)
                {
                    this.HandleException(StatusCode.Exception);
                }
                return PhotonSocketError.Exception;
            }

            return PhotonSocketError.Success;
        }


        /// <summary>Not used currently.</summary>
        public override PhotonSocketError Receive(out byte[] data)
        {
            data = null;
            return PhotonSocketError.NoData;
        }

        /// <summary>Used by TPeer to receive.</summary>
        public IEnumerator ReceiveLoop()
        {
            try
            {
                this.sock.Connect();
            }
            catch (Exception e)
            {
                if (this.State != PhotonSocketState.Disconnecting && this.State != PhotonSocketState.Disconnected)
                {
                    if (this.ReportDebugOfLevel(DebugLevel.ERROR))
                    {
                        this.EnqueueDebugReturn(DebugLevel.ERROR, "Receive issue. State: " + this.State + ". Server: '" + ServerAddress + "' Exception: " + e);
                    }

                    this.HandleException(StatusCode.ExceptionOnReceive);
                }
            }

            while (this.State == PhotonSocketState.Connecting && this.sock != null && !this.sock.IsAlive)
            {
                #if UNITY_5_3 || UNITY_5_3_OR_NEWER
                yield return new WaitForRealSeconds(0.02f);
                #else
                float waittime = Time.realtimeSinceStartup + 0.2f;
                while (Time.realtimeSinceStartup < waittime) yield return 0;
                #endif
            }

            if (this.sock == null)
            {
                if (this.State != PhotonSocketState.Disconnecting && this.State != PhotonSocketState.Disconnected)
                {
                    this.Listener.DebugReturn(DebugLevel.ERROR, "Exiting receive thread. Server: " + ServerAddress);
                    this.HandleException(StatusCode.ExceptionOnConnect);
                }
                yield break;
            }

            // connected
            this.State = PhotonSocketState.Connected;
            // peerBase.OnConnect();


            byte[] inBuff = null;

            // receiving
            while (this.State == PhotonSocketState.Connected)
            {
                try
                {
                    if (this.State != PhotonSocketState.Disconnecting && this.State != PhotonSocketState.Disconnected)
                    {
                        this.Listener.DebugReturn(DebugLevel.ERROR, "Exiting receive thread (inside loop). Server: " + ServerAddress);
                        this.HandleException(StatusCode.ExceptionOnReceive);
                    }

                    // inBuff = this.sock.;
                }
                catch (Exception e)
                {
                    if (this.State != PhotonSocketState.Disconnecting && this.State != PhotonSocketState.Disconnected)
                    {
                        if (this.ReportDebugOfLevel(DebugLevel.ERROR))
                        {
                            this.EnqueueDebugReturn(DebugLevel.ERROR, "Receive issue. State: " + this.State + ". Server: '" + ServerAddress + "' Exception: " + e);
                        }

                        this.HandleException(StatusCode.ExceptionOnReceive);
                    }
                }


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
                if (inBuff.Length < 0)
                {
                    // got disconnected (from remote or net)
                    if (this.State != PhotonSocketState.Disconnecting && this.State != PhotonSocketState.Disconnected)
                    {
                        this.HandleException(StatusCode.DisconnectByServer);
                    }
                    break;
                }

                try
                {
                    if (this.ReportDebugOfLevel(DebugLevel.ALL))
                    {
                        this.Listener.DebugReturn(DebugLevel.ALL, "TCP << " + inBuff.Length + " = " + SupportClassPun.ByteArrayToString(inBuff));
                    }

                    this.HandleReceivedDatagram(inBuff, inBuff.Length, false);
                }
                catch (Exception e)
                {
                    if (this.State != PhotonSocketState.Disconnecting && this.State != PhotonSocketState.Disconnected)
                    {
                        if (this.ReportDebugOfLevel(DebugLevel.ERROR))
                        {
                            this.EnqueueDebugReturn(DebugLevel.ERROR, "Receive issue. State: " + this.State + ". Server: '" + ServerAddress + "' Exception: " + e);
                        }

                        this.HandleException(StatusCode.ExceptionOnReceive);
                    }
                }
            }


            this.Disconnect();
        }
    }

    internal class MonoBehaviourExt : MonoBehaviour { }
}

// #endif
