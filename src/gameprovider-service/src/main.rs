mod models;
use models::*;
use serde_json::Result;

fn main() -> Result<()> {
    let data = r#"
    {
        "name": "Game 1",
        "ip": "127.0.0.1",
        "port": 1025
    }"#;

    let game: models::Game = serde_json::from_str(&data)?;
    let mut gameprovider = GameProvider::new();
    gameprovider.add(Game::new(game.name, game.ip, game.port));

    for x in gameprovider.get_all() {
        println!("Server: {} IP: {}:{}", x.name, x.ip, x.port);
    }

    Ok(())
}
