mod models;
use models::GameProvider;
use serde_json;
use std::fs::File;
use std::path::Path;

fn get_gameprovider(path: &str) -> GameProvider {
    let json_file_path = Path::new(path);
    let json_file = File::open(json_file_path).expect("File not found");
    let gameprovider: GameProvider =
        serde_json::from_reader(json_file).expect("Could not read json");

    return gameprovider;
}

fn main() {
    let gameprovider = get_gameprovider("games.json");
    for game in gameprovider.get_all() {
        println!("Server: {} IP: {}:{}", game.name, game.ip, game.port);
    }
}
