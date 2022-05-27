# Character Service

[![Character Service Build](https://github.com/codingben/maple-fighters/actions/workflows/character-service-build.yml/badge.svg)](https://github.com/codingben/maple-fighters/actions/workflows/character-service-build.yml)

Character service stores the player's character data.

## Project Structure

```
├── migrations
└── src
    ├── db
    ├── handlers
    └── models
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

You should now be able to access it at `http://localhost:50053`.
