use actix_web::{web::Data, App, Responder, HttpResponse, HttpServer};
use dotenv::dotenv;
use serde::{Serialize, Deserialize};
use std::{env, io};

#[derive(Serialize, Deserialize)]
#[serde(rename_all = "camelCase")]
struct Config {
    game_services: Vec<GameService>,
}

#[derive(Serialize, Deserialize, Clone)]
struct GameService {
    name: String,
    environment: String,
    protocol: String,
    url: String,
}

async fn read_config_from_remote() -> Result<Config, reqwest::Error> {
    let config_url = format!(
        "https://raw.githubusercontent.com/codingben/maple-fighters-configs/{}/game-services.yml",
        env::var("CONFIG_SOURCE").unwrap()
    );
    let content = reqwest::get(&config_url)
        .await?
        .text()
        .await?;
    let config: Config = serde_yaml::from_str(&content)
        .unwrap();

    Ok(config)
}

fn config_to_game_services(config: Config, environment: &str) -> Vec<GameService> {
    config
        .game_services
        .into_iter()
        .filter(|service| service.environment == environment)
        .collect()
}

#[actix_web::get("/games")]
async fn get_game_services(data: Data<Vec<GameService>>) -> impl Responder {
    HttpResponse::Ok().json(data.get_ref())
}

#[actix_web::main]
async fn main() -> io::Result<()> {
    dotenv().ok();
    env_logger::init();

    let ip_address = env::var("IP_ADDRESS").expect("IP_ADDRESS not found");
    let environment = env::var("CONFIG_ENVIRONMENT").expect("CONFIG_ENVIRONMENT not found");
    let config = read_config_from_remote().await.unwrap();
    let data = Data::new(config_to_game_services(config, &environment));

    HttpServer::new(move || {
        App::new()
            .app_data(data.clone())
            .service(get_game_services)
    })
    .bind(ip_address)?
    .run()
    .await
}
