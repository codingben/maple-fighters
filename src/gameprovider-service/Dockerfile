FROM rust:latest as builder
LABEL builder=true

COPY target/release /usr/target/release

FROM debian:buster-slim

COPY --from=builder /usr/target/release/gameprovider-service /
COPY data.json /tmp

CMD ["/gameprovider-service"]