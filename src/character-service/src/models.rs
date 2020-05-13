use diesel::pg::PgConnection;
use diesel::prelude::*;

use super::schema::characters;
use super::schema::characters::dsl::characters as all_characters;

#[derive(Queryable)]
pub struct Character {
    pub id: i32,
    pub userid: i32,
    pub charactername: String,
    pub index: i32,
    pub classindex: i32,
}

#[derive(Insertable)]
#[table_name = "characters"]
pub struct NewCharacter {
    pub userid: i32,
    pub charactername: String,
    pub index: i32,
    pub classindex: i32,
}

impl Character {
    pub fn insert(character: NewCharacter, conn: &PgConnection) -> bool {
        diesel::insert_into(characters::table)
            .values(&character)
            .execute(conn)
            .is_ok()
    }

    pub fn delete(id: i32, conn: &PgConnection) -> bool {
        diesel::delete(all_characters.find(id))
            .execute(conn)
            .is_ok()
    }

    pub fn get_all(userid: i32, conn: &PgConnection) -> Vec<Character> {
        all_characters
            .filter(characters::userid.eq(userid))
            .load::<Character>(conn)
            .expect(&format!("Error loading characters for user id {}", userid))
    }
}
