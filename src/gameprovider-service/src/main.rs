use actix_web::{middleware::Logger, web, App, HttpServer, Responder};
use dotenv::dotenv;
use std::{env, io::Result};

struct AppData {
    game_services: String,
}

async fn get_game_services(data: web::Data<AppData>) -> impl Responder {
    data.get_ref().game_services.clone()
}

#[actix_web::main]
async fn main() -> Result<()> {
    dotenv().ok();

    env_logger::init();

    let ip_address = env::var("IP_ADDRESS").expect("IP_ADDRESS not found");
    let game_services = env::var("GAME_SERVICES").expect("GAME_SERVICES not found");
    let data = web::Data::new(AppData {
        game_services: game_services,
    });

    HttpServer::new(move || {
        App::new()
            .app_data(data.clone())
            .wrap(Logger::default())
            .route("/games", web::get().to(get_game_services))
    })
    .bind(ip_address)?
    .run()
    .await
}
