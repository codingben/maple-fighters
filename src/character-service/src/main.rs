#[macro_use]
extern crate diesel;
extern crate dotenv;

mod endpoints;
mod models;
mod schema;
mod character {
    tonic::include_proto!("character");
}

use character::character_server::CharacterServer;
use diesel::{pg::PgConnection, r2d2::ConnectionManager, r2d2::Pool};
use dotenv::dotenv;
use endpoints::CharacterService;
use std::{env, error::Error};
use tonic::transport::Server;

#[tokio::main]
async fn main() -> Result<(), Box<dyn Error>> {
    dotenv().expect("Could not find .env file");

    let address = env::var("IP_ADDRESS").expect("IP_ADDRESS not found");
    let database_url = env::var("DATABASE_URL").expect("DATABASE_URL not found");
    let manager = ConnectionManager::<PgConnection>::new(database_url);
    let r2d2_pool = Pool::builder()
        .build(manager)
        .expect("Failed to create pool");

    let character_service = CharacterService { pool: r2d2_pool };
    let address_parsed = address.parse()?;

    println!("Server is running {}", address);

    Server::builder()
        .add_service(CharacterServer::new(character_service))
        .serve(address_parsed)
        .await?;

    Ok(())
}
