use super::game_provider::{game_provider_server::GameProvider, *};
use tonic::{Request, Response, Status};

#[derive(Debug, Default)]
pub struct GameProviderService {
    pub game_servers: Vec<Game>,
}

#[tonic::async_trait]
impl GameProvider for GameProviderService {
    async fn get_game_servers(
        &self,
        _request: Request<()>,
    ) -> Result<Response<GetGameServersResponse>, Status> {
        Ok(Response::new(GetGameServersResponse {
            game_collection: self.game_servers.to_vec(),
        }))
    }
}
