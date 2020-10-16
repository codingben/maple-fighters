FROM rust:latest as builder
LABEL builder=true

COPY target/release /usr/target/release

FROM debian:buster-slim

COPY --from=builder /usr/target/release/gameprovider-service /
COPY data.json /tmp

ENV RUST_LOG=info
ENV RUST_BACKTRACE=full
ENV IP_ADDRESS=0.0.0.0:50052
ENV DATABASE_PATH=tmp/data.json

EXPOSE 50052

CMD ["/gameprovider-service"]