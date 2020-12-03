use actix_web::{middleware::Logger, web, App, HttpServer};
use dotenv::dotenv;
use models::GameCollection;
use std::{env::var, io::Result, sync::Arc};

mod database;
mod handlers;
mod models;

#[actix_rt::main]
async fn main() -> Result<()> {
    dotenv().ok();

    env_logger::init();

    let address = var("IP_ADDRESS").expect("IP_ADDRESS not found");
    let data_path = var("DATABASE_PATH").expect("DATABASE_PATH not found");

    println!("Server is running {}", address);

    HttpServer::new(move || {
        App::new()
            .wrap(Logger::default())
            .data(Arc::new(GameCollection {
                collection: database::load(&data_path),
            }))
            .route("/games", web::get().to(handlers::get_game_servers))
    })
    .bind(address)?
    .run()
    .await
}
