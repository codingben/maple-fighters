To use WebSockets with the Photon C# library, you need to import this folder into your project.

    SocketWebTcpThread      can be used in all cases where the Thread class is available
    SocketWebTcpCoroutine   must be used for WebGL exports and when the Thread class is unavailable

    WebSocket.cs            is used in all exports
    websocket-sharp.dll     is used when not exporting to a browser (and in Unity Editor)
    WebSocket.jslib         is used for WebGL exports by Unity (and must be setup accordingly)


You might have to define "WEBSOCKET" for your project to make the classes available.
To make a connection us WebSocket, setup the PhotonPeer (LoadBalancingPeer, ChatPeer, etc) similar to this:

        Debug.Log("WSS Setup");
        PhotonPeer.TransportProtocol = ConnectionProtocol.WebSocket;    // or WebSocketSecure for a release
        PhotonPeer.SocketImplementationConfig[ConnectionProtocol.WebSocket] = typeof(SocketWebTcpThread);
        PhotonPeer.SocketImplementationConfig[ConnectionProtocol.WebSocketSecure] = typeof(SocketWebTcpThread);

        //PhotonPeer.DebugOut = DebugLevel.INFO;	// this would show some logs from the SocketWebTcp implementation