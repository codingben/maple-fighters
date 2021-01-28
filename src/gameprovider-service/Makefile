build:
	docker build -t gameprovider-service:1.0.0 .

run:
	docker run -e RUST_LOG=info \
			   -e RUST_BACKTRACE=1 \
			   -e IP_ADDRESS=0.0.0.0:50052 \
			   -p 50052:50052 gameprovider-service:1.0.0