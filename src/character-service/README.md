# Character Service
The character service creates and receives the player character data.

## Docker
Follow these instructions to create an image and run a container:

1. Build a docker image (The image size should be around 78.4MB):
```bash
make build
```

2. Running in a docker container:
```bash
make run
```
You should now be able to access it at `http://localhost:50053`.