# Game Application

Game application creates a WebSocket server and handles requests. When a new client connects, it creates a new player in the game world.

## Project Structure

```
├── Components
│   ├── Collection
│   ├── Generator
│   └── Scene
├── Coroutines
├── Handlers
├── Messages
├── MessageTools
├── Objects
│   ├── Mob
│   │   └── Components
│   ├── Npc
│   │   └── Components
│   ├── Player
│   │   └── Components
│   └── Portal
│       └── Components
├── Physics
```

## Quickstart

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

You should now be able to access it at `ws://localhost:50051`.
