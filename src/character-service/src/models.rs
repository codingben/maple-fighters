use super::schema::characters;

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
