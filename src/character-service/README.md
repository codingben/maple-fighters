# Character Service
The character service creates and receives the player character data.

## Docker
> 💡 You need to install Rust locally.

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