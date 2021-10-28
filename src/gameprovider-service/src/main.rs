use actix_web::{middleware::Logger, web, App, HttpServer, Responder};
use dotenv::dotenv;
use std::{env, io::Result};

async fn get_games() -> impl Responder {
    env::var("GAMES").expect("GAMES not found")
}

#[actix_web::main]
async fn main() -> Result<()> {
    dotenv().ok();

    env_logger::init();

    let ip_address = env::var("IP_ADDRESS").expect("IP_ADDRESS not found");

    HttpServer::new(move || {
        App::new()
            .wrap(Logger::default())
            .route("/games", web::get().to(get_games))
    })
    .bind(ip_address)?
    .run()
    .await
}
