use crate::database::*;
use crate::models::NewCharacter;
use actix_web::{web, web::Json, web::Path, Error, HttpResponse};

pub async fn create(pool: web::Data<Pool>, data: Json<NewCharacter>) -> Result<HttpResponse, Error> {
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

pub async fn delete_by_id(pool: web::Data<Pool>, id: Path<i32>) -> Result<HttpResponse, Error> {
    let conn = pool.get().unwrap();
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

pub async fn get_all(pool: web::Data<Pool>, id: Path<i32>) -> Result<HttpResponse, Error> {
    let conn = pool.get().unwrap();
    let user_id = id.into_inner();
    let characters = web::block(move || get_all_by_user_id(user_id, &conn))
        .await
        .map_err(|_| HttpResponse::InternalServerError().finish())?;

    Ok(HttpResponse::Ok().json(&characters))
}
