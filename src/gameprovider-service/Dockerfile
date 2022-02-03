FROM rust:1.54 as builder
COPY ./ ./
RUN cargo build --release

FROM rust:1.54
ENV NAME=gameprovider-service
COPY --from=build ./target/release/${NAME} /usr/local/bin/${NAME}
CMD ["gameprovider-service"]