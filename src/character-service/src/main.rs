#[macro_use]
extern crate diesel;
extern crate dotenv;

pub mod models;
pub mod schema;

use diesel::pg::PgConnection;
use diesel::prelude::*;
use dotenv::dotenv;
use std::env;

use tonic::{transport::Server, Request, Response, Status};

use character::character_server::{Character, CharacterServer};
use character::create_response::CharacterCreationStatus;
use character::remove_response::CharacterRemoveStatus;
use character::*;

mod character {
    tonic::include_proto!("character");
}

#[derive(Debug, Default)]
struct CharacterData {}

#[tonic::async_trait]
impl Character for CharacterData {
    async fn create(
        &self,
        _request: tonic::Request<CreateRequest>,
    ) -> Result<tonic::Response<CreateResponse>, tonic::Status> {
        Ok(Response::new(CreateResponse {
            character_creation_status: CharacterCreationStatus::Failed as i32,
        }))
    }

    async fn remove(
        &self,
        _request: tonic::Request<RemoveRequest>,
    ) -> Result<tonic::Response<RemoveResponse>, tonic::Status> {
        Ok(Response::new(RemoveResponse {
            character_remove_status: CharacterRemoveStatus::Failed as i32,
        }))
    }

    async fn get_all(
        &self,
        _request: tonic::Request<GetAllRequest>,
    ) -> Result<tonic::Response<GetAllResponse>, tonic::Status> {
        Ok(Response::new(GetAllResponse {
            character_collection: Vec::new(),
        }))
    }

    async fn update(
        &self,
        _request: tonic::Request<UpdateRequest>,
    ) -> Result<tonic::Response<()>, tonic::Status> {
        Ok(Response::new(()))
    }
}

fn main() {
    dotenv().expect("Could not find .env file");

    let database_url = env::var("DATABASE_URL").expect("DATABASE_URL not found");
    let conn = PgConnection::establish(&database_url).unwrap();

    let character = models::NewCharacter {
        userid: 1,
        charactername: String::from("benzuk"),
        index: 1,
        classindex: 0,
        mapindex: 0,
    };

    if models::Character::insert(character, &conn) {
        println!("insert::succeed");
    } else {
        println!("insert::failed");
    }

    if models::Character::delete(1, &conn) {
        println!("delete::succeed");
    } else {
        println!("delete::failed");
    }
}
