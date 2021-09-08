# Build app
FROM rust:1.54 as build
COPY ./ ./
RUN cargo build --release

# Copy build artifacts
FROM gcr.io/distroless/cc-debian10 as runtime
ENV NAME=gameprovider-service
COPY --from=build ./target/release/${NAME} /usr/local/bin/${NAME}
CMD ["gameprovider-service"]