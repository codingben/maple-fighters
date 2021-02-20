# Maple Fighters
![CI](https://github.com/benukhanov/maple-fighters/workflows/CI/badge.svg?branch=develop)

A small online game similar to MapleStory.

## About

Maple Fighters is a multiplayer online game inspired by MapleStory. Players can choose a fighter to travel the world and fight monsters with other fighters.

Please **★ Star** this repository if you like it and find it useful. Made With ❤ For Open Source Community!

## Gameplay

Feel free to play the demo [here](https://ukben.dev/maple-fighters). Please note that it is currently offline version.

## Screenshots

| Lobby                                                                                                         | The Dark Forest                                                                                                    |
| ----------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------ |
| <img src="docs/lobby.jpg"> | <img src="docs/the-dark-forest.jpg"> |

## Technology

**Game Engine**: Unity WebGL   
**Client**: C# (In Unity C# is compiled to C++ and then to WebAssembly)   
**Server**: C#, Rust   
**Database**: MongoDB, PostgreSQL   
**Reverse Proxy**: Nginx   

## Microservices Architecture

| Service                                              | Language      | Description                                                    														|
| ---------------------------------------------------- | ------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [auth-service](./src/auth-service)                   | C#            | Stores user data and verifies user. 			   															|
| [game-service](./src/game-service/Game.Application)                   | C#            | Creates a player in the game world to play with others. 	|
| [gameprovider-service](./src/gameprovider-service)   | Rust          | Provides a list of game servers. 																|
| [character-service](./src/character-service)         | Rust          | Creates and receives player character data. 																|

## Quickstart
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
You should be able to access it at `http://localhost`.

## Contributing
Feel free to contribute and make any changes to the game itself.

Please follow the [Conventional Commits](https://www.conventionalcommits.org/) specification.

## Artwork
The artwork is owned by Nexon Co., Ltd and will never be used commercially.

## License
[AGPL](https://choosealicense.com/licenses/agpl-3.0/)
