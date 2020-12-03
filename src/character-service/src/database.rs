use crate::models::*;
use crate::schema::characters;
use crate::schema::characters::dsl::characters as db_characters;
use diesel::{pg::PgConnection, prelude::*, r2d2, r2d2::ConnectionManager, result::Error};

pub type Pool = r2d2::Pool<ConnectionManager<PgConnection>>;

pub fn insert(character: NewCharacter, conn: &PgConnection) -> Result<bool, Error> {
    if is_name_used(character.userid, &character.charactername, &conn)? {
        return Ok(false);
    }

    let is_ok = diesel::insert_into(characters::table)
        .values(&character)
        .execute(conn)
        .is_ok();

    Ok(is_ok)
}

pub fn delete(id: i32, conn: &PgConnection) -> Result<bool, Error> {
    if get_by_id(id, conn)?.is_empty() {
        return Ok(false);
    };

    let is_ok = diesel::delete(db_characters.find(id)).execute(conn).is_ok();

    Ok(is_ok)
}

pub fn get_by_id(id: i32, conn: &PgConnection) -> Result<Vec<Character>, Error> {
    db_characters.find(id).load::<Character>(conn)
}

pub fn get_all_by_user_id(userid: i32, conn: &PgConnection) -> Result<Vec<Character>, Error> {
    db_characters
        .filter(characters::userid.eq(userid))
        .load::<Character>(conn)
}

pub fn is_name_used<'a>(userid: i32, name: &'a str, conn: &PgConnection) -> Result<bool, Error> {
    for character in get_all_by_user_id(userid, &conn)? {
        if character.charactername == name {
            return Ok(true);
        }
    }

    Ok(false)
}
