mod game_provider {
    tonic::include_proto!("game_provider");
}
mod database;
mod models;

use dotenv::dotenv;
use game_provider::{
    game_provider_server::{GameProvider, GameProviderServer},
    *,
};
use std::{env, error::Error};
use tonic::{transport::Server, Request, Response, Status};

#[derive(Debug, Default)]
pub struct GameProviderImpl {
    game_servers: Vec<Game>,
}

#[tonic::async_trait]
impl GameProvider for GameProviderImpl {
    async fn get_game_servers(
        &self,
        _request: Request<()>,
    ) -> Result<Response<GetGameServersResponse>, Status> {
        Ok(Response::new(GetGameServersResponse {
            game_collection: self.game_servers.to_vec(),
        }))
    }
}

#[tokio::main]
async fn main() -> Result<(), Box<dyn Error>> {
    dotenv().expect("Could not find .env file");

    let address = env::var("IP_ADDRESS").expect("IP_ADDRESS not found");
    let data_path = env::var("DATABASE_PATH").expect("DATABASE_PATH not found");
    let address_parsed = address.parse()?;
    let mut game_provider = GameProviderImpl::default();
    let game_server_collection = database::GameServerCollection::new(&data_path);
    for game_server in game_server_collection.get_all() {
        game_provider.game_servers.push(Game {
            name: game_server.name.clone(),
            ip: game_server.ip.clone(),
            port: game_server.port.clone(),
        });
    }

    Server::builder()
        .add_service(GameProviderServer::new(game_provider))
        .serve(address_parsed)
        .await?;

    Ok(())
}
