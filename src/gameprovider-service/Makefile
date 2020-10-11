build:
	cargo build --release
	docker build -t gameprovider-service:1.0 .

clean:
	docker rmi `docker images --filter label=builder=true -q`

run:
	docker run -p 50053:50053 gameprovider-service:1.0