using System;
using System.Collections.Generic;
using CommonTools.Coroutines;
using CommonTools.Log;
using ExitGames.Client.Photon;

namespace PhotonClientImplementation
{
    public static class PeerUtils
    {
        public static IEnumerator<IYieldInstruction> WaitForDisconnect(this PhotonPeer peer)
        {
            do
            {
                // Left blank intentionally
                yield return null;
            }
            while (peer.State != PeerStateValue.Disconnected);
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
                    LogUtils.Break(MessageBuilder.Trace("Unexpected state while waiting for connection: " + peer.State));
                    break;
                }
            }
        }
    }
}