use crate::models::GameCollection;
use actix_web::web::Data;
use actix_web::HttpResponse;
use std::sync::Arc;

pub fn get_game_servers(data: Data<Arc<GameCollection>>) -> HttpResponse {
    let data = data.clone();
    HttpResponse::Ok().json(&data.game_collection)
}
