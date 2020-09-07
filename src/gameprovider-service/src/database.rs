use super::models::Game;
use serde_json;
use std::{fs::File, path::Path};

pub fn load(path: &str) -> Vec<Game> {
    let json_file_path = Path::new(path);
    let json_file = File::open(json_file_path).expect("File not found");
    let json_data: Vec<super::models::Game> =
        serde_json::from_reader(&json_file).expect("Could not read json");

    json_data
        .into_iter()
        .map(|game| Game {
            name: game.name,
            ip: game.ip,
            port: game.port,
        })
        .collect()
}
