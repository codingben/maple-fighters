use crate::database::*;
use crate::models::NewCharacter;
use actix_web::{web, web::Json, web::Path, Error, HttpResponse};

pub fn create_new(db: web::Data<Pool>, character: Json<NewCharacter>) -> HttpResponse {
    let conn = db.get().unwrap();
    let new_character = character.into_inner();

    if is_character_name_used(new_character.userid, &new_character.charactername, &conn) {
        HttpResponse::BadRequest().json("Please choose a different character name.")
    } else {
        if insert_character(new_character, &conn) {
            HttpResponse::Created().finish()
        } else {
            HttpResponse::InternalServerError().finish()
        }
    }
}

pub async fn remove_by_id(db: web::Data<Pool>, id: Path<i32>) -> Result<HttpResponse, Error> {
    let conn = db.get().unwrap();
    let character_id = id.into_inner();
    let is_deleted = web::block(move || delete_character(character_id, &conn))
        .await
        .map_err(|e| HttpResponse::InternalServerError().finish())?;

    if is_deleted {
        Ok(HttpResponse::Ok().finish())
    } else {
        Ok(HttpResponse::NotFound().json("The character was not found."))
    }
}

pub async fn get_all(db: web::Data<Pool>, id: Path<i32>) -> Result<HttpResponse, Error> {
    let characters = web::block(move || {
        let user_id = id.into_inner();
        let conn = db.get().unwrap();

        get_characters_by_user_id(user_id, &conn)
    })
    .await
    .map_err(|e| HttpResponse::InternalServerError().finish())?;

    Ok(HttpResponse::Ok().json(&characters))
}
