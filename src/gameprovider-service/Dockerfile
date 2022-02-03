FROM rust:1.54 as builder
COPY ./ ./
RUN cargo build --release

FROM gcr.io/distroless/cc-debian10
ENV NAME=gameprovider-service
COPY --from=builder ./target/release/${NAME} /usr/local/bin/${NAME}
CMD ["gameprovider-service"]