use crate::models::*;
use crate::schema::characters;
use crate::schema::characters::dsl::characters as all_characters;
use diesel::{pg::PgConnection, prelude::*, r2d2, r2d2::ConnectionManager, result::Error};

pub type Pool = r2d2::Pool<ConnectionManager<PgConnection>>;

pub fn insert_character(character: NewCharacter, conn: &PgConnection) -> Result<bool, Error> {
    let is_ok = diesel::insert_into(characters::table)
        .values(&character)
        .execute(conn)
        .is_ok();

    Ok(is_ok)
}

pub fn delete_character(id: i32, conn: &PgConnection) -> Result<bool, Error> {
    if get_character_by_id(id, conn)?.is_empty() {
        return Ok(false);
    };

    let is_ok = diesel::delete(all_characters.find(id))
        .execute(conn)
        .is_ok();

    Ok(is_ok)
}

pub fn get_character_by_id(id: i32, conn: &PgConnection) -> Result<Vec<Character>, Error> {
    let result = all_characters.find(id).load::<Character>(conn)?;

    Ok(result)
}

pub fn get_characters_by_user_id(
    userid: i32,
    conn: &PgConnection,
) -> Result<Vec<Character>, Error> {
    let result = all_characters
        .filter(characters::userid.eq(userid))
        .load::<Character>(conn)?;

    Ok(result)
}

pub fn is_character_name_used<'a>(
    userid: i32,
    name: &'a str,
    conn: &PgConnection,
) -> Result<bool, Error> {
    let result = get_characters_by_user_id(userid, &conn)?;

    for character in result {
        if character.charactername == name {
            return Ok(true);
        }
    }

    Ok(false)
}
