# Docker Instructions
Follow these instructions to create an image and run a container.

1. Create a single file deployment:
```bash
dotnet publish --runtime alpine-x64 -c Release --self-contained true -o ./publish /p:PublishSingleFile=true /p:PublishTrimmed=true
```
2. Build a docker image:
```bash
docker build -t auth-service:v1 .
```
3. Running in a docker container:
```bash
docker run -p 50050:50050 auth-service:v1
```
You should now be able to access it at `http://localhost:50050`.