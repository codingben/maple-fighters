mod models;
use models::*;

fn main() {
    let mut game_provider = GameProvider { games: Vec::new() };
    game_provider.add(Game::new("My Game", "127.0.0.1", 1001));

    for game in game_provider.get_all() {
        println!("Server: {} IP: {}:{}", game.name, game.ip, game.port);
    }
}
