use crate::database::*;
use crate::models::new_character::NewCharacter;

use actix_web::{web, web::Data, web::Json, Error, HttpResponse};

pub async fn create(pool: Data<Pool>, data: Json<NewCharacter>) -> Result<HttpResponse, Error> {
    let conn = pool.get().unwrap();
    let character_data = data.into_inner();
    let is_inserted = web::block(move || insert(character_data, &conn))
        .await
        .map_err(|_| HttpResponse::InternalServerError().finish())?;

    if is_inserted {
        Ok(HttpResponse::Created().finish())
    } else {
        Ok(HttpResponse::BadRequest().json("Please choose a different character name."))
    }
}
