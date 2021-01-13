# Maple Fighters
![CI](https://github.com/benukhanov/maple-fighters/workflows/CI/badge.svg?branch=develop)

Maple Fighters is a small online game similar to MapleStory. The goal is to make a minimalistic, simple and effective game using the latest technology.

Please **★ Star** this repository if you like it and find it useful. Made with ❤ for Open Source Community!

## Gameplay
| Lobby                                                                                                         | The Dark Forest                                                                                                    |
| ----------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------ |
| <img src="docs/Lobby.png"> | <img src="docs/The Dark Forest.png"> |

## Technology

**Client**: Unity WebGL/WebAssembly   
**Server**: C#, Rust   
**Database**: MongoDB, PostgreSQL   
**Reverse Proxy**: Nginx   

## Microservices Architecture

| Service                                              | Language      | Description                                                    														|
| ---------------------------------------------------- | ------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [auth-service](./src/auth-service)                   | C#            | Stores user data and verifies user. 			   															|
| [game-service](./src/game-service)                   | C#            | Creates a player in the game world to play with others. 	|
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
You should be able to access it at `http://localhost:8000`.

## Contributing
Feel free to contribute and make any changes to the game itself.

Please follow the [Conventional Commits](https://www.conventionalcommits.org/) specification.

## License
[AGPL](https://choosealicense.com/licenses/agpl-3.0/)
