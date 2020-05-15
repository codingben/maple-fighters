use serde::Deserialize;
use serde_json;
use std::{fs::File, path::Path};

#[derive(Deserialize)]
pub struct GameServer {
    pub name: String,
    pub ip: String,
    pub port: i32,
}

#[allow(dead_code)]
impl GameServer {
    pub fn new(name: String, ip: String, port: i32) -> GameServer {
        GameServer {
            name: name,
            ip: ip,
            port: port,
        }
    }
}

#[derive(Deserialize)]
pub struct GameServerCollection {
    pub game_servers: Vec<GameServer>,
}

#[allow(dead_code)]
impl GameServerCollection {
    pub fn new(path: &str) -> GameServerCollection {
        let json_file_path = Path::new(path);
        let json_file = File::open(json_file_path).expect("File not found");
        let gameprovider: GameServerCollection =
            serde_json::from_reader(json_file).expect("Could not read json");
        return gameprovider;
    }

    pub fn add(&mut self, game_server: GameServer) {
        self.game_servers.push(game_server);
    }

    pub fn get_all(&self) -> impl Iterator<Item = &GameServer> {
        return self.game_servers.iter();
    }
}
