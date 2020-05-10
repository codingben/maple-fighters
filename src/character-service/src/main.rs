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

    async fn get(
        &self,
        _request: tonic::Request<GetRequest>,
    ) -> Result<tonic::Response<GetResponse>, tonic::Status> {
        Ok(Response::new(GetResponse {
            character_data: None,
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
    println!("Hello, world!");
}
