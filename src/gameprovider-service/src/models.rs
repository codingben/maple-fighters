use serde::Deserialize;

#[derive(Deserialize)]
pub struct Game {
    pub name: String,
    pub ip: String,
    pub port: i32,
}

#[allow(dead_code)]
impl Game {
    pub fn new(name: String, ip: String, port: i32) -> Game {
        Game {
            name: name,
            ip: ip,
            port: port,
        }
    }
}
