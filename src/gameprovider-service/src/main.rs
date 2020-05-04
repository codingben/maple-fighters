use tonic::{transport::Server, Request, Response, Status};

use game_provider::game_collection::Game;
use game_provider::game_provider_server::{GameProvider, GameProviderServer};
use game_provider::GameCollection;

use dotenv::dotenv;
use std::env;
use std::error::Error;

mod game_provider {
    tonic::include_proto!("game_provider");
}

mod models;

#[derive(Debug, Default)]
pub struct GameProviderData {
    game_servers: Vec<Game>,
}

#[tonic::async_trait]
impl GameProvider for GameProviderData {
    async fn get_games(&self, _request: Request<()>) -> Result<Response<GameCollection>, Status> {
        Ok(Response::new(GameCollection {
            games: self.game_servers.to_vec(),
        }))
    }
}

#[tokio::main]
async fn main() -> Result<(), Box<dyn Error>> {
    dotenv().expect("Could not find .env file");

    let address = env::var("IP_ADDRESS").expect("IP_ADDRESS not found");
    let path = env::var("GAME_SERVER_DATA_PATH").expect("GAME_SERVER_DATA_PATH not found");
    let address_parsed = address.parse()?;
    let mut game_provider_data = GameProviderData::default();
    let game_server_collection = models::GameServerCollection::new(&path);
    for game_server in game_server_collection.get_all() {
        game_provider_data.game_servers.push(Game {
            name: game_server.name.clone(),
            ip: game_server.ip.clone(),
            port: game_server.port.clone(),
        });
    }

    Server::builder()
        .add_service(GameProviderServer::new(game_provider_data))
        .serve(address_parsed)
        .await?;

    Ok(())
}
