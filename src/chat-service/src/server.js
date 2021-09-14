const WebSocket = require("ws");

require("dotenv").config();

var server = new WebSocket.Server({ port: process.env.PORT });
server.on("connection", onConnection);
server.on("close", onClose);

function onConnection(webSocket) {
  webSocket.on("message", (data, isBinary) => {
    server.clients.forEach((client) => {
      if (client === webSocket || client.readyState !== WebSocket.OPEN) {
        return;
      }

      client.send(data, { binary: isBinary });
    });
  });

  console.log("A new client connected.");
}

function onClose() {
  console.log("The client has been disconnected.");
}
