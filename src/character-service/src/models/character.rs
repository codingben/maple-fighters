use serde::Serialize;

#[derive(Queryable, Serialize, Debug)]
pub struct Character {
    pub id: i32,
    pub userid: String,
    pub charactername: String,
    pub index: i32,
    pub classindex: i32,
}
