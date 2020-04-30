pub struct Game<'a> {
    pub name: &'a str,
    pub ip: &'a str,
    pub port: i32,
}

impl<'a> Game<'a> {
    pub fn new(name: &'a str, ip: &'a str, port: i32) -> Game<'a> {
        Game {
            name: name,
            ip: ip,
            port: port,
        }
    }
}

pub struct GameProvider<'a> {
    pub games: Vec<Game<'a>>,
}

impl<'a> GameProvider<'a> {
    pub fn add(&mut self, game: Game<'a>) {
        self.games.push(game);
    }

    pub fn get_all(&self) -> impl Iterator<Item = &Game<'a>> {
        self.games.iter()
    }
}
