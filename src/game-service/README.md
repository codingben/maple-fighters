# Game Service
The game service creates a player in the game world to play with others. Also creates objects of the game world (e.g. mobs, NPCs, etc.).

## Docker
> 💡 You need to install the .NET Core SDK locally.

Follow these instructions to create an image and run a container:

1. Create a single file deployment:
```bash
make publish
```
2. Build a docker image (The image size should be around 108MB):
```bash
make build
```
3. Running in a docker container:
```bash
make run
```
You should now be able to access it at `http://localhost:50051`.