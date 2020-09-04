# Maple Fighters
The server-side will be able to accommodate many players who can play together in the game world.

## Technology

**Client**: Unity WebGL   
**Server**: C#, Rust   
**Database**: MongoDB, PostgreSQL   

## Service Architecture

| Service                                              | Language      | Description                                                    														|
| ---------------------------------------------------- | ------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [auth-service](./src/auth-service)                   | C#            | Stores user data and verifies user. 			   															|
| [game-service](./src/game-service)                   | C#            | Creates a player entity in the game world to play with others. It also creates player characters and objects of the game world (such as mobs, non-player characters, etc.). 	|
| [gameprovider-service](./src/gameprovider-service)   | Rust          | Provides the list of game servers. 																|
| [character-service](./src/character-service)         | Rust          | Creates player character data and receives it. 																|
