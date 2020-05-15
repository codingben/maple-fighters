#[macro_use]
extern crate diesel;
extern crate dotenv;

mod character {
    tonic::include_proto!("character");
}
mod models;
mod schema;

use character::{
    character_server::{Character, CharacterServer},
    *,
};
use diesel::{pg::PgConnection, r2d2};
use dotenv::dotenv;
use r2d2::{ConnectionManager, Pool};
use std::{env, error::Error};
use tonic::{transport::Server, Request, Response, Status};

struct CharacterImpl {
    pool: Pool<ConnectionManager<PgConnection>>,
}

#[tonic::async_trait]
impl Character for CharacterImpl {
    async fn create(
        &self,
        request: Request<CreateRequest>,
    ) -> Result<Response<CreateResponse>, Status> {
        let create_request = request.into_inner();
        let user_id = create_request.user_id;
        let character_data = create_request.character_data;
        if let Some(new_character) = character_data {
            let character = models::NewCharacter {
                userid: user_id,
                charactername: new_character.name,
                index: new_character.index,
                classindex: new_character.class_index,
            };
            let connection = self.pool.get().unwrap();
            let status: create_response::CharacterCreationStatus;

            if models::Character::insert(character, &connection) {
                status = create_response::CharacterCreationStatus::Succeed;
            } else {
                status = create_response::CharacterCreationStatus::Failed;
            }

            Ok(Response::new(CreateResponse {
                character_creation_status: status as i32,
            }))
        } else {
            Err(Status::invalid_argument("Invalid character data"))
        }
    }

    async fn remove(
        &self,
        request: Request<RemoveRequest>,
    ) -> Result<Response<RemoveResponse>, Status> {
        let remove_request = request.into_inner();
        let connection = self.pool.get().unwrap();
        let status: remove_response::CharacterRemoveStatus;

        if models::Character::delete(remove_request.id, &connection) {
            status = remove_response::CharacterRemoveStatus::Succeed;
        } else {
            status = remove_response::CharacterRemoveStatus::Failed;
        }

        Ok(Response::new(RemoveResponse {
            character_remove_status: status as i32,
        }))
    }

    async fn get_all(
        &self,
        request: Request<GetAllRequest>,
    ) -> Result<Response<GetAllResponse>, Status> {
        let remove_request = request.into_inner();
        let connection = self.pool.get().unwrap();
        let characters = models::Character::get_all(remove_request.user_id, &connection);
        let mut collection = Vec::new();
        for character in characters {
            collection.push(character::CharacterData {
                name: character.charactername,
                index: character.index,
                class_index: character.classindex,
            })
        }

        Ok(Response::new(GetAllResponse {
            character_collection: collection.to_vec(),
        }))
    }
}

#[tokio::main]
async fn main() -> Result<(), Box<dyn Error>> {
    dotenv().expect("Could not find .env file");

    let address = env::var("IP_ADDRESS").expect("IP_ADDRESS not found");
    let database_url = env::var("DATABASE_URL").expect("DATABASE_URL not found");
    let manager = r2d2::ConnectionManager::<PgConnection>::new(database_url);
    let pool = r2d2::Pool::builder()
        .build(manager)
        .expect("Failed to create pool");

    let character = CharacterImpl { pool: pool };
    let address_parsed = address.parse()?;

    Server::builder()
        .add_service(CharacterServer::new(character))
        .serve(address_parsed)
        .await?;

    Ok(())
}
