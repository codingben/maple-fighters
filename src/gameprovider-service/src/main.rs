use actix_web::{App, HttpServer};
use dotenv::dotenv;
use std::{env, io::Result};

mod database;
mod handlers;
mod models;

#[actix_rt::main]
async fn main() -> Result<()> {
    dotenv().expect("Could not find .env file");

    let address = env::var("IP_ADDRESS").expect("IP_ADDRESS not found");
    let data_path = env::var("DATABASE_PATH").expect("DATABASE_PATH not found");
    // TODO: Use: database::load(&data_path)

    println!("Server is running {}", address);

    HttpServer::new(move || App::new())
        .bind(address)?
        .run()
        .await
}
