use crate::db::characters;
use crate::db::characters::Pool;

use actix_web::{web, web::Path, Error, HttpResponse};

pub async fn get(pool: web::Data<Pool>, id: Path<i32>) -> Result<HttpResponse, Error> {
    let conn = pool.get().unwrap();
    let user_id = id.into_inner();
    let characters = web::block(move || characters::get_all_by_user_id(user_id, &conn))
        .await
        .map_err(|_| HttpResponse::InternalServerError().finish())?;

    Ok(HttpResponse::Ok().json(&characters))
}
