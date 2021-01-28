use serde::Serialize;

#[derive(Queryable, Serialize, Debug)]
pub struct Character {
    pub id: i32,
    pub userid: i32,
    pub charactername: String,
    pub index: i32,
    pub classindex: i32,
}
