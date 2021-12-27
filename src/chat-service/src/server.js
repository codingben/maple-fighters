const WebSocket = require("ws");

require("dotenv").config();

var port = process.env.PORT;
var server = new WebSocket.Server({ port: port });
server.on("connection", (webSocket) => {
  webSocket.on("message", (data, isBinary) => {
    server.clients.forEach((client) => {
      if (client === webSocket || client.readyState !== webSocket.OPEN) {
        return;
      }

      client.send(data, { binary: isBinary });
    });
  });

  console.log("A new client connected.");
});
server.on("close", () => {
  console.log("The client has been disconnected.");
});

console.log("Server started, listening on port %s", port);
