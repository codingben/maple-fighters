use actix_web::{web::Data, web::Json, App, Responder, HttpServer};
use dotenv::dotenv;
use serde::{Serialize, Deserialize};
use serde_yaml::from_str;
use std::{env, io};

#[derive(Serialize, Deserialize)]
#[serde(rename_all = "camelCase")]
struct Config {
    game_services: Vec<GameService>,
}

#[derive(Serialize, Deserialize)]
struct GameService {
    name: String,
    protocol: String,
    url: String,
}

async fn read_config_from_remote() -> Result<Config, reqwest::Error> {
    let config_url = format!(
        "https://raw.githubusercontent.com/codingben/maple-fighters-configs/{}/game-services.yml",
        env::var("CONFIG_SOURCE").unwrap()
    );
    let content = reqwest::get(config_url)
        .await?
        .text()
        .await?;

    Ok(from_str(&content).unwrap())
}

#[actix_web::get("/games")]
async fn get_game_services(config: Data<Config>) -> impl Responder {
    Json(config)
}

#[actix_web::main]
async fn main() -> io::Result<()> {
    dotenv().ok();
    env_logger::init();

    let ip_address = env::var("IP_ADDRESS").expect("IP_ADDRESS not found");
    let config = Data::new(read_config_from_remote().await.unwrap());

    HttpServer::new(move || {
        App::new()
            .app_data(config.clone())
            .service(get_game_services)
    })
    .bind(ip_address)?
    .run()
    .await
}
