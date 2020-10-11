build:
	cargo build --release
	docker build -t gameprovider-service:1.0 .
	docker rmi `docker images --filter label=builder=true -q`

run:
	docker run -p 50052:50052 gameprovider-service:1.0