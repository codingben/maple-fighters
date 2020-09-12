use crate::database::Pool;
use crate::models::NewCharacter;
use actix_web::{web, web::Json, web::Path, HttpResponse};

pub fn create_new(db: web::Data<Pool>, character: Json<NewCharacter>) -> HttpResponse {
    HttpResponse::Ok().finish()
}

pub fn remove_by_id(db: web::Data<Pool>, id: Path<i32>) -> HttpResponse {
    HttpResponse::Ok().finish()
}

pub fn get_all(db: web::Data<Pool>) -> HttpResponse {
    HttpResponse::Ok().finish()
}
