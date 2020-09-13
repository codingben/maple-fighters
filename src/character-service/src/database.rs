use crate::models::*;
use crate::schema::characters;
use crate::schema::characters::dsl::characters as all_characters;
use diesel::{pg::PgConnection, prelude::*, r2d2, r2d2::ConnectionManager};

pub type Pool = r2d2::Pool<ConnectionManager<PgConnection>>;

pub fn insert_character(character: NewCharacter, conn: &PgConnection) -> bool {
    diesel::insert_into(characters::table)
        .values(&character)
        .execute(conn)
        .is_ok()
}

pub fn delete_character(id: i32, conn: &PgConnection) -> bool {
    if get_character_by_id(id, conn).is_empty() {
        return false;
    };

    diesel::delete(all_characters.find(id))
        .execute(conn)
        .is_ok()
}

pub fn get_character_by_id(id: i32, conn: &PgConnection) -> Vec<Character> {
    all_characters
        .find(id)
        .load::<Character>(conn)
        .expect(&format!("Error loading character for id {}", id))
}

pub fn get_characters_by_user_id(userid: i32, conn: &PgConnection) -> Vec<Character> {
    all_characters
        .filter(characters::userid.eq(userid))
        .load::<Character>(conn)
        .expect(&format!("Error loading characters for user id {}", userid))
}

pub fn is_character_name_used<'a>(userid: i32, name: &'a str, conn: &PgConnection) -> bool {
    for character in get_characters_by_user_id(userid, &conn) {
        if character.charactername == name {
            return true;
        }
    }

    return false;
}
