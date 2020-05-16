use serde::Deserialize;

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
