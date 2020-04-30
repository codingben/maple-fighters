mod models;
use models::*;

fn main() {
    let mut game_provider = GameProvider { games: Vec::new() };
    game_provider
        .games
        .push(Game::new("My Game", "127.0.0.1", 1001));

    for game in game_provider.games.iter() {
        println!("Server: {} IP: {}:{}", game.name, game.ip, game.port);
    }
}
