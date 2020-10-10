# Auth Service
The authentication service stores user data and validates the user.

## Docker
> 💡 You need to install the .NET Core SDK locally.

Follow these instructions to create an image and run a container:

1. Create a single file deployment:
```bash
dotnet publish --runtime alpine-x64 -c Release --self-contained true -o ./publish /p:PublishSingleFile=true /p:PublishTrimmed=true
```
2. Build a docker image (The image size should be around 67.5MB):
```bash
docker build -t auth-service:v1 .
```
3. Running in a docker container:
```bash
docker run -p 50050:50050 auth-service:v1
```
You should now be able to access it at `http://localhost:50050`.