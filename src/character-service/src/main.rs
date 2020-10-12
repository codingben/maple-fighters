#[macro_use]
extern crate diesel;
extern crate dotenv;

use actix_web::{middleware::Logger, web, App, HttpServer};
use diesel::{pg::PgConnection, r2d2::ConnectionManager, r2d2::Pool};
use dotenv::dotenv;
use std::{env::var, io::Result};

mod database;
mod handlers;
mod models;
mod schema;

#[actix_rt::main]
async fn main() -> Result<()> {
    dotenv().ok();

    env_logger::init();

    let address = var("IP_ADDRESS").expect("IP_ADDRESS not found");
    let database_url = var("DATABASE_URL").expect("DATABASE_URL not found");
    let manager = ConnectionManager::<PgConnection>::new(database_url);
    let r2d2_pool = Pool::builder()
        .build(manager)
        .expect("Failed to create pool");

    println!("Server is running {}", address);

    HttpServer::new(move || {
        App::new()
            .wrap(Logger::default())
            .data(r2d2_pool.clone())
            .route("/characters", web::post().to(handlers::create))
            .route("/characters/{id}", web::delete().to(handlers::delete_by_id))
            .route("/characters/{id}", web::get().to(handlers::get_all))
    })
    .bind(address)?
    .run()
    .await
}
