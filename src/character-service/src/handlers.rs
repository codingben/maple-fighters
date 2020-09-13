use crate::database::*;
use crate::models::NewCharacter;
use actix_web::{web, web::Json, web::Path, HttpResponse};

pub fn create_new(db: web::Data<Pool>, character: Json<NewCharacter>) -> HttpResponse {
    let conn = db.get().unwrap();
    let data = character.into_inner();

    if is_character_name_used(data.userid, &data.charactername, &conn) {
        HttpResponse::BadRequest().json("Please choose a different character name.")
    } else {
        if insert_character(data, &conn) {
            HttpResponse::Created().finish()
        } else {
            HttpResponse::InternalServerError().finish()
        }
    }
}

pub fn remove_by_id(db: web::Data<Pool>, id: Path<i32>) -> HttpResponse {
    HttpResponse::Ok().finish()
}

pub fn get_all(db: web::Data<Pool>) -> HttpResponse {
    HttpResponse::Ok().finish()
}
