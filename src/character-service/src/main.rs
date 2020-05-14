#[macro_use]
extern crate diesel;
extern crate dotenv;

pub mod models;
pub mod schema;

use diesel::pg::PgConnection;
use diesel::prelude::*;
use diesel::r2d2;
use dotenv::dotenv;
use std::env;
use std::error::Error;

use tonic::{transport::Server, Request, Response, Status};

use character::character_server::{Character, CharacterServer};
use character::create_response::CharacterCreationStatus;
use character::remove_response::CharacterRemoveStatus;
use character::*;

mod character {
    tonic::include_proto!("character");
}

struct CharacterImpl {
    conn: r2d2::Pool<r2d2::ConnectionManager<PgConnection>>,
}

#[tonic::async_trait]
impl Character for CharacterImpl {
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
}

#[tokio::main]
async fn main() -> Result<(), Box<dyn Error>> {
    dotenv().expect("Could not find .env file");

    let database_url = env::var("DATABASE_URL").expect("DATABASE_URL not found");
    let manager = r2d2::ConnectionManager::<PgConnection>::new(database_url);
    let pool = r2d2::Pool::builder()
        .build(manager)
        .expect("Failed to create pool.");

    let character = CharacterImpl { conn: pool };
    let address = "0.0.0.0:50054";
    let address_parsed = address.parse()?;

    Server::builder()
        .add_service(CharacterServer::new(character))
        .serve(address_parsed)
        .await?;

    Ok(())
}
