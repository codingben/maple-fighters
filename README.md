# Maple Fighters
This is a small game similar to MapleStory. 

- Click [here](https://maplefighters.io/) to play (Coming soon).

The client side is built on Unity and should be compatible with all platforms (e.g. PC, WebGL). And the server side will be able to accommodate many players who can play together in the game world.

## Technology

**Client**: Unity WebGL   
**Server**: C#, Rust   
**Database**: MongoDB, PostgreSQL   
**Reverse Proxy**: Nginx   

## Service Architecture

| Service                                              | Language      | Description                                                    														|
| ---------------------------------------------------- | ------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [maple-fighters](./src/maple-fighters)                   | C            | Exposes an HTTP server for serving the game's website.		   															|
| [auth-service](./src/auth-service)                   | C#            | Stores user data and verifies user. 			   															|
| [game-service](./src/game-service)                   | C#            | Creates a player in the game world to play with others. 	|
| [gameprovider-service](./src/gameprovider-service)   | Rust          | Provides a list of game servers. 																|
| [character-service](./src/character-service)         | Rust          | Creates and receives player character data. 																|

## Running locally
> 💡 You need to install Docker and Docker Compose locally.

Follow these instructions to start and stop locally:

1. To create containers:
```bash
docker-compose up
```

2. To stop and remove containers:
```bash
docker-compose down
```
You should now be able to access it at `http://localhost:8000`.

## Lobby

<img src="docs/Lobby.png">

## The Dark Forest

<img src="docs/The Dark Forest.png">
