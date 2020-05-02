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

#[derive(Deserialize)]
pub struct GameProvider {
    pub games: Vec<Game>,
}

#[allow(dead_code)]
impl GameProvider {
    pub fn new() -> GameProvider {
        GameProvider { games: Vec::new() }
    }

    pub fn add(&mut self, game: Game) {
        self.games.push(game);
    }

    pub fn get_all(&self) -> impl Iterator<Item = &Game> {
        self.games.iter()
    }
}
