FROM rust:1.47.0 as builder
LABEL builder=true

COPY target/release /usr/target/release

FROM debian:buster-slim

COPY --from=builder /usr/target/release/character-service /

ENV RUST_LOG=info
ENV RUST_BACKTRACE=full
ENV IP_ADDRESS=0.0.0.0:50053
ENV DATABASE_URL=postgres://postgres:postgres@localhost:5432/postgres

EXPOSE 50053

CMD ["/character-service"]