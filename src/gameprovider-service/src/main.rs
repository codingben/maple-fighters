mod models;
use models::*;

fn main() {
    let mut gameprovider = GameProvider::new();
    gameprovider.add(Game::new("My Game", "127.0.0.1", 1001));

    for game in gameprovider.get_all() {
        println!("Server: {} IP: {}:{}", game.name, game.ip, game.port);
    }
}
