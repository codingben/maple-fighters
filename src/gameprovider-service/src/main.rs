struct Game<'a> {
    name: &'a str,
    ip: &'a str,
    port: i32,
}

impl<'a> Game<'a> {
    fn new(name: &'a str, ip: &'a str, port: i32) -> Game<'a> {
        Game {
            name: name,
            ip: ip,
            port: port,
        }
    }
}

struct GameProvider<'a> {
    games: Vec<Game<'a>>,
}

fn main() {
    let mut game_provider = GameProvider { games: Vec::new() };
    game_provider
        .games
        .push(Game::new("My Game", "127.0.0.1", 1001));

    for x in game_provider.games.iter() {
        println!("Server: {} IP: {}:{}", x.name, x.ip, x.port);
    }
}
