build:
	cargo build --release
	docker build -t character-service:1.0.0 .
	docker rmi `docker images --filter label=builder=true -q`

run:
	docker run -p 50053:50053 character-service:1.0.0