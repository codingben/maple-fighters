#[macro_use]
extern crate diesel;
extern crate dotenv;

use actix_web::App;
use actix_web::HttpServer;
use diesel::{pg::PgConnection, r2d2::ConnectionManager, r2d2::Pool};
use dotenv::dotenv;
use std::{env, io::Result};

mod database;
mod models;
mod schema;

#[actix_rt::main]
async fn main() -> Result<()> {
    dotenv().expect("Could not find .env file");

    let address = env::var("IP_ADDRESS").expect("IP_ADDRESS not found");
    let database_url = env::var("DATABASE_URL").expect("DATABASE_URL not found");
    let manager = ConnectionManager::<PgConnection>::new(database_url);
    let r2d2_pool = Pool::builder()
        .build(manager)
        .expect("Failed to create pool");

    println!("Server is running {}", address);

    HttpServer::new(move || App::new().data(r2d2_pool.clone()))
        .bind(address)?
        .run()
        .await
}
