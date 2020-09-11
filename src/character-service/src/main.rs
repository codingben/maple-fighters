#[macro_use]
extern crate diesel;
extern crate dotenv;

use diesel::{pg::PgConnection, r2d2::ConnectionManager, r2d2::Pool};
use dotenv::dotenv;
use std::env;

mod database;
mod models;
mod schema;

fn main() {
    dotenv().expect("Could not find .env file");

    let address = env::var("IP_ADDRESS").expect("IP_ADDRESS not found");
    let database_url = env::var("DATABASE_URL").expect("DATABASE_URL not found");
    let manager = ConnectionManager::<PgConnection>::new(database_url);
    let r2d2_pool = Pool::builder()
        .build(manager)
        .expect("Failed to create pool");

    println!("Server is running {}", address);
}
