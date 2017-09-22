#if WEBSOCKET
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SocketTcp.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// <summary>
//   Internal class to encapsulate the network i/o functionality for the realtime libary.
// </summary>
// <author>developer@photonengine.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Threading;


namespace ExitGames.Client.Photon
{
    /// <summary>
    /// Internal class to encapsulate the network i/o functionality for the realtime libary.
    /// </summary>
    public class SocketWebTcpThread : IPhotonSocket, IDisposable
    {
        private WebSocket sock;

        private readonly object syncer = new object();

        public SocketWebTcpThread(PeerBase npeer) : base(npeer)
        {
            if (this.ReportDebugOfLevel(DebugLevel.INFO))
            {
                this.EnqueueDebugReturn(DebugLevel.INFO, "new SocketWebTcpThread(). Server: " + this.ConnectAddress + " protocol: " + this.Protocol);
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

        public override bool Connect()
        {
            bool baseOk = base.Connect();
            if (!baseOk)
            {
                return false;
            }

            this.State = PhotonSocketState.Connecting;

            Thread dns = new Thread(this.DnsAndConnect);
            dns.Name = "photon dns thread";
            dns.IsBackground = true;
            dns.Start();

            return true;
        }

        internal void DnsAndConnect()
        {
            try
            {
                lock (this.syncer)
                {
                    IPAddress ipAddress = IPhotonSocket.GetIpAddress(this.ServerAddress);
                    if (ipAddress == null)
                    {
                        throw new ArgumentException("DNS failed to resolve for address: " + this.ServerAddress);
                    }

                    this.AddressResolvedAsIpv6 = ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6;

                    this.sock = new WebSocket(new Uri(this.ConnectAddress));
                }
            }
            catch (SecurityException se)
            {
                if (this.ReportDebugOfLevel(DebugLevel.ERROR))
                {
                    this.Listener.DebugReturn(DebugLevel.ERROR, "Connect() to '" + this.ConnectAddress + "' failed: " + se.ToString());
                }

                this.HandleException(StatusCode.SecurityExceptionOnConnect);
                return;
            }
            catch (Exception se)
            {
                if (this.ReportDebugOfLevel(DebugLevel.ERROR))
                {
                    this.Listener.DebugReturn(DebugLevel.ERROR, "Connect() to '" + this.ConnectAddress + "' failed: " + se.ToString());
                }

                this.HandleException(StatusCode.ExceptionOnConnect);
                return;
            }

            Thread run = new Thread(new ThreadStart(this.ReceiveLoop));
            run.Name = "photon receive thread";
            run.IsBackground = true;
            run.Start();
        }

        public override bool Disconnect()
        {
            if (ReportDebugOfLevel(DebugLevel.INFO))
            {
                this.Listener.DebugReturn(DebugLevel.INFO, "SocketWebTcpThread.Disconnect()");
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
                    this.Listener.DebugReturn(DebugLevel.ALL, "Sending: " + SupportClass.ByteArrayToString(data));
                }
                this.sock.Send(data);
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

        public void ReceiveLoop()
        {
            this.sock.Connect();

            while (!this.sock.Connected && this.sock.Error == null)
            {
                Thread.Sleep(0);
            }
            if (this.sock.Error != null)
            {
                this.Listener.DebugReturn(DebugLevel.ERROR, "Exiting receive thread. Server: " + this.ConnectAddress + " Error: " + this.sock.Error);
				this.HandleException(StatusCode.ExceptionOnConnect);
            }
            else
            {
                this.State = PhotonSocketState.Connected;
                this.peerBase.OnConnect();

				while (this.State == PhotonSocketState.Connected)
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
                            Thread.Sleep(0);
							continue;
						}

						if (this.ReportDebugOfLevel(DebugLevel.ALL))
						{
							this.Listener.DebugReturn(DebugLevel.ALL, "TCP << " + inBuff.Length + " = " + SupportClass.ByteArrayToString(inBuff));
						}

						if (inBuff.Length > 0)
						{
							try
							{
							    this.HandleReceivedDatagram(inBuff, inBuff.Length, false);
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

            this.Disconnect();
        }
    }
}

#endif