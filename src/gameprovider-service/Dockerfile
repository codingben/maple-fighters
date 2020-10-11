FROM ekidd/rust-musl-builder:stable AS build
COPY . ./

RUN sudo chown -R rust:rust .
RUN cargo build --release

FROM scratch
COPY --from=build /home/rust/src/target/x86_64-unknown-linux-musl/release/gameprovider-service /

ENV PORT 50053

EXPOSE ${PORT}
CMD ["/gameprovider-service"]