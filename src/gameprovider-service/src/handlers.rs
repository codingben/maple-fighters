use super::models::GameCollection;
use actix_web::web::Data;
use actix_web::HttpResponse;
use std::sync::Arc;

pub fn get_game_servers(data: Data<Arc<GameCollection>>) -> HttpResponse {
    let game_collection = data.clone();
    HttpResponse::Ok().json(&game_collection.collection)
}
