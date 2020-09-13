use crate::database::*;
use crate::models::NewCharacter;
use actix_web::{web, web::Json, web::Path, HttpResponse};

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

pub fn remove_by_id(db: web::Data<Pool>, id: Path<i32>) -> HttpResponse {
    let conn = db.get().unwrap();
    let character_id = id.into_inner();

    if delete_character(character_id, &conn) {
        HttpResponse::Ok().finish()
    } else {
        HttpResponse::NotFound().json("The character was not found.")
    }
}

pub fn get_all(db: web::Data<Pool>, id: Path<i32>) -> HttpResponse {
    let conn = db.get().unwrap();
    let user_id = id.into_inner();
    let characters = get_characters_by_user_id(user_id, &conn);

    HttpResponse::Ok().json(characters)
}
