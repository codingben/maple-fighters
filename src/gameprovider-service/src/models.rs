use serde::{Deserialize, Serialize};

#[derive(Serialize)]
pub struct GameCollection {
    pub collection: Vec<Game>,
}

#[derive(Serialize, Deserialize)]
pub struct Game {
    pub name: String,
    pub ip: String,
    pub port: i32,
}
