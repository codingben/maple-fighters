use actix_web::HttpResponse;

pub async fn healthz() -> HttpResponse {
    HttpResponse::Ok().body("Healthy")
}
