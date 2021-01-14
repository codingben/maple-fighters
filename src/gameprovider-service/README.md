# GameProvider Service
The game provider service provides a list of game servers.

## Docker
Follow these instructions to create an image and run a container:

1. Build a docker image (The image size should be around 77.7MB):
```bash
make build
```

2. Running in a docker container:
```bash
make run
```
You should now be able to access it at `http://localhost:50052`.