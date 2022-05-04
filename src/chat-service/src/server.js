const WebSocket = require("ws");

require("dotenv").config();

var port = process.env.PORT;
var server = new WebSocket.Server({ port: port });
server.on("connection", onConnection);

function onConnection(connection) {
  connection.on("message", (data, isBinary) =>
    onMessage(connection, data, isBinary)
  );
}

function onMessage(connection, data, isBinary) {
  server.clients.forEach((client) => {
    if (client === connection || client.readyState !== connection.OPEN) {
      return;
    }

    client.send(data, { binary: isBinary });
  });
}

console.log("Server started, listening on port %s", port);
