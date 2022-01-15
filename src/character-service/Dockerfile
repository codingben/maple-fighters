FROM rust:1.54 as builder
COPY ./ ./
RUN cargo build --release

FROM rust:1.54
ENV NAME=character-service
COPY --from=builder ./target/release/${NAME} /usr/local/bin/${NAME}
CMD ["character-service"]