# Game Service

The game service creates a player in the game world to play with others. Also creates objects of the game world (e.g. mobs, NPCs, etc.).

## Project Structure

```
├── Components
│   ├── Collection
│   └── Scene
├── Coroutines
├── Handlers
├── Messages
├── MessageTools
├── Objects
│   ├── BlueSnail
│   │   └── Components
│   ├── Guardian
│   │   └── Components
│   ├── Player
│   │   └── Components
│   └── Portal
│       └── Components
└── Physics
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
