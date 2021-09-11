const WebSocket = require("ws");

require("dotenv").config();

const server = new WebSocket.Server({ port: process.env.PORT });

server.on("connection", function connection(_) {
  console.log("A new client connected.");
});
