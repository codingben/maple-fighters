# Chat Service

[![Chat Service Build](https://github.com/benukhanov/maple-fighters/actions/workflows/chat-service-build.yml/badge.svg)](https://github.com/benukhanov/maple-fighters/actions/workflows/chat-service-build.yml)

The chat service is a service that allows players to communicate with each other.

### Docker

Follow these instructions to create an image and run a container:

1. Build a docker image:

```bash
make build
```

2. Running in a docker container:

```bash
make run
```

You should now be able to access it at `http://localhost:50054`.
