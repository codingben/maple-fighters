use crate::database::*;
use crate::models::NewCharacter;
use actix_web::{web, web::Json, web::Path, Error, HttpResponse};

pub async fn create_new(
    db: web::Data<Pool>,
    character: Json<NewCharacter>,
) -> Result<HttpResponse, Error> {
    let conn = db.get().unwrap();
    let new_character = character.into_inner();
    let is_created = web::block(move || insert(new_character, &conn))
        .await
        .map_err(|_| HttpResponse::InternalServerError().finish())?;

    if is_created {
        Ok(HttpResponse::Created().finish())
    } else {
        Ok(HttpResponse::BadRequest().json("Please choose a different character name."))
    }
}

pub async fn delete_by_id(db: web::Data<Pool>, id: Path<i32>) -> Result<HttpResponse, Error> {
    let conn = db.get().unwrap();
    let character_id = id.into_inner();
    let is_deleted = web::block(move || delete(character_id, &conn))
        .await
        .map_err(|_| HttpResponse::InternalServerError().finish())?;

    if is_deleted {
        Ok(HttpResponse::Ok().finish())
    } else {
        Ok(HttpResponse::NotFound().json("The character was not found."))
    }
}

pub async fn get_all(db: web::Data<Pool>, id: Path<i32>) -> Result<HttpResponse, Error> {
    let user_id = id.into_inner();
    let conn = db.get().unwrap();
    let characters = web::block(move || get_all_by_user_id(user_id, &conn))
        .await
        .map_err(|_| HttpResponse::InternalServerError().finish())?;

    Ok(HttpResponse::Ok().json(&characters))
}
