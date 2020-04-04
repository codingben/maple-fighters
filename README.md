# Server-side Projects
It is planned that the server-side will be able to accommodate a lot of players who can play together in a world. My goal is to create (for study purposes) a game similar to MapleStory (MMO).

# Server-side Architecture

| Service                                              | Language      | Description                                                    |
| ---------------------------------------------------- | ------------- | ---------------------------------------------------------------|
| [auth-service](./src/auth-service)                   | C#            | Stores user data and verifies user. 			   	                  |
| [game-service](./src/game-service)                   | C#            | Creates the player in the game world to play with others. 	    |
| [gameprovider-service](./src/gameprovider-service)   | Rust          | Provides the list of game servers from a JSON file. 		        |
