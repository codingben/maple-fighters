use crate::db::characters;
use crate::db::characters::Pool;
use crate::models::new_character::NewCharacter;

use actix_web::{web, web::Data, web::Json, Error, HttpResponse};

pub async fn create(pool: Data<Pool>, data: Json<NewCharacter>) -> Result<HttpResponse, Error> {
    let conn = pool.get().unwrap();
    let character_data = data.into_inner();
    let userid = character_data.userid.to_owned();
    let name = character_data.charactername.to_owned();
    let characters = characters::get_all_by_user_id(userid, &conn);

    if characters.is_ok() {
        for character in characters.unwrap() {
            if character.charactername == name {
                return Ok(
                    HttpResponse::BadRequest().json("Please choose a different character name.")
                );
            }
        }
    }

    let is_created = web::block(move || characters::create(character_data, &conn))
        .await
        .map_err(|_| HttpResponse::InternalServerError().finish())?;

    if is_created {
        Ok(HttpResponse::Created().finish())
    } else {
        Ok(HttpResponse::InternalServerError().json("Failed to create character."))
    }
}
