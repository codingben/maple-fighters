# Auth Service
The authentication service stores user data and validates the user.

## Docker
> 💡 You need to install the .NET Core SDK locally.

Follow these instructions to create an image and run a container:

1. Create a single file deployment:
```bash
make publish
```
2. Build a docker image (The image size should be around 67.5MB):
```bash
make build
```
3. Running in a docker container:
```bash
make run
```
You should now be able to access it at `http://localhost:50050`.