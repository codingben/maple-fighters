use crate::schema::characters;
use serde::Deserialize;

#[derive(Insertable, Deserialize, Debug)]
#[table_name = "characters"]
pub struct NewCharacter {
    pub userid: i32,
    pub charactername: String,
    pub index: i32,
    pub classindex: i32,
}
