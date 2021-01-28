use crate::models::character::Character;
use crate::models::new_character::NewCharacter;
use crate::schema::characters;
use crate::schema::characters::dsl::characters as db_characters;

use diesel::{pg::PgConnection, prelude::*, r2d2, r2d2::ConnectionManager, result::Error};

pub type Pool = r2d2::Pool<ConnectionManager<PgConnection>>;

pub fn create(character: NewCharacter, conn: &PgConnection) -> Result<bool, Error> {
    Ok(diesel::insert_into(characters::table)
        .values(&character)
        .execute(conn)
        .is_ok())
}

pub fn delete_by_id(id: i32, conn: &PgConnection) -> Result<bool, Error> {
    Ok(diesel::delete(db_characters.find(id)).execute(conn).is_ok())
}

pub fn get_by_id(id: i32, conn: &PgConnection) -> Result<Vec<Character>, Error> {
    db_characters.find(id).load::<Character>(conn)
}

pub fn get_all_by_user_id(userid: i32, conn: &PgConnection) -> Result<Vec<Character>, Error> {
    db_characters
        .filter(characters::userid.eq(userid))
        .load::<Character>(conn)
}
