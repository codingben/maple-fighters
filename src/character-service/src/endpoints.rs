use super::character::{character_server::Character, *};
use super::models;
use diesel::{pg::PgConnection, r2d2};
use r2d2::{ConnectionManager, Pool};
use tonic::{Request, Response, Status};

pub struct CharacterService {
    pub pool: Pool<ConnectionManager<PgConnection>>,
}

#[tonic::async_trait]
impl Character for CharacterService {
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
            let is_name_already_in_use = models::Character::is_name_already_in_use(
                character.userid,
                &character.charactername,
                &connection,
            );

            if is_name_already_in_use {
                status = create_response::CharacterCreationStatus::NameAlreadyInUse;
            } else {
                if models::Character::insert(character, &connection) {
                    status = create_response::CharacterCreationStatus::Succeed;
                } else {
                    status = create_response::CharacterCreationStatus::Failed;
                }
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
        let id = remove_request.id;
        let connection = self.pool.get().unwrap();
        let status: remove_response::CharacterRemoveStatus;

        if models::Character::delete(id, &connection) {
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
        let user_id = remove_request.user_id;
        let connection = self.pool.get().unwrap();
        let characters = models::Character::get_by_user_id(user_id, &connection);
        let mut collection = Vec::new();
        for character in characters {
            collection.push(get_all_response::CharacterData {
                id: character.id,
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
