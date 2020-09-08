use crate::models::GameCollection;
use actix_web::{web, App, HttpServer};
use dotenv::dotenv;
use std::{env, io::Result, sync::Arc};

mod database;
mod handlers;
mod models;

#[actix_rt::main]
async fn main() -> Result<()> {
    dotenv().expect("Could not find .env file");

    let address = env::var("IP_ADDRESS").expect("IP_ADDRESS not found");
    let data_path = env::var("DATABASE_PATH").expect("DATABASE_PATH not found");

    println!("Server is running {}", address);

    HttpServer::new(move || {
        App::new()
            .data(Arc::new(GameCollection {
                collection: database::load(&data_path),
            }))
            .route("/games", web::get().to(handlers::get_game_servers))
    })
    .bind(address)?
    .run()
    .await
}
