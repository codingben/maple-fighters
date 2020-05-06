# Server-side Projects
It is planned that the server-side will be able to accommodate a lot of players who can play together in a world. My goal is to create (for study purposes) a game similar to MapleStory (MMO).

# Server-side Architecture

| Service                                              | Language      | Description                                                    														|
| ---------------------------------------------------- | ------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [auth-service](./src/auth-service)                   | C#            | Stores user data and verifies user. 			   															|
| [game-service](./src/game-service)                   | C#            | Creates a player entity in the game world to play with others. It also creates player characters and objects of the game world (such as mobs, non-player characters, etc.). 	|
| [gameprovider-service](./src/gameprovider-service)   | Rust          | Provides the list of game servers. 																|
| [character-service](./src/character-service)         | Rust          | Stores player character data. 																|
