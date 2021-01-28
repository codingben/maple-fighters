use actix_web::{middleware::Logger, web, App, HttpServer};
use dotenv::dotenv;
use std::{env, io::Result};

async fn get_games() -> &'static str {
    "[{\"name\":\"Europe\",\"protocol\":\"ws\",\"url\":\"localhost/game/\"}]"
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
