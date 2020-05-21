mod database;
mod endpoints;
mod models;
mod game_provider {
    tonic::include_proto!("game_provider");
}

use database::GameServerCollection;
use dotenv::dotenv;
use endpoints::GameProviderService;
use game_provider::{game_provider_server::GameProviderServer, *};
use std::{env, error::Error};
use tonic::transport::Server;

#[tokio::main]
async fn main() -> Result<(), Box<dyn Error>> {
    dotenv().expect("Could not find .env file");

    let address = env::var("IP_ADDRESS").expect("IP_ADDRESS not found");
    let data_path = env::var("DATABASE_PATH").expect("DATABASE_PATH not found");
    let address_parsed = address.parse()?;

    let mut game_provider_service = GameProviderService::default();
    let game_server_collection = GameServerCollection::new(&data_path);
    for game_server in game_server_collection.get_all() {
        game_provider_service.game_servers.push(Game {
            name: game_server.name.clone(),
            ip: game_server.ip.clone(),
            port: game_server.port.clone(),
        });
    }

    Server::builder()
        .add_service(GameProviderServer::new(game_provider_service))
        .serve(address_parsed)
        .await?;

    Ok(())
}
