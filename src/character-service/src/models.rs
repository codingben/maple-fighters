use diesel;
use diesel::pg::PgConnection;
use diesel::prelude::*;

use schema::characters;
use schema::characters::dsl::characters as all_characters;
use schema::characters::dsl::*;

#[derive(Queryable)]
pub struct Character {
    pub id: i32,
    pub userid: i32,
    pub charactername: String,
    pub index: i32,
    pub classindex: i32,
    pub mapindex: i32,
}

#[derive(Insertable)]
#[table_name = "characters"]
pub struct NewCharacter {
    pub userid: i32,
    pub charactername: String,
    pub index: i32,
    pub classindex: i32,
    pub mapindex: i32,
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
}
