use models;
use web;

pub fn create_character(db: web<Pool>, character: Json<NewCharacter>) -> HttpResponse {}

pub fn remove_character(db: web<Pool>, id: Path<i32>) -> HttpResponse {}

pub fn get_all_characters(db: web<Pool>) -> HttpResponse {}
