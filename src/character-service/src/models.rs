use super::schema::characters;
use super::schema::characters::dsl::characters as all_characters;
use diesel::{pg::PgConnection, prelude::*};

#[derive(Queryable, Debug)]
pub struct Character {
    pub id: i32,
    pub userid: i32,
    pub charactername: String,
    pub index: i32,
    pub classindex: i32,
}

#[derive(Insertable, Debug)]
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
        if Character::get_by_id(id, conn).is_empty() {
            return false;
        };

        diesel::delete(all_characters.find(id))
            .execute(conn)
            .is_ok()
    }

    pub fn get_by_id(id: i32, conn: &PgConnection) -> Vec<Character> {
        all_characters
            .find(id)
            .load::<Character>(conn)
            .expect(&format!("Error loading character for id {}", id))
    }

    pub fn get_by_user_id(userid: i32, conn: &PgConnection) -> Vec<Character> {
        all_characters
            .filter(characters::userid.eq(userid))
            .load::<Character>(conn)
            .expect(&format!("Error loading characters for user id {}", userid))
    }

    pub fn is_name_already_in_use<'a>(userid: i32, name: &'a str, conn: &PgConnection) -> bool {
        for character in Character::get_by_user_id(userid, &conn) {
            if character.charactername == name {
                return true;
            }
        }

        return false;
    }
}
