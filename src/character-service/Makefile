build:
	docker build -t character-service:1.0.0 .

run:
	docker run -e RUST_LOG=info \
			   -e RUST_BACKTRACE=1 \
			   -e IP_ADDRESS=0.0.0.0:50053 \
			   -e DATABASE_URL=postgres://postgres:postgres@localhost:5432/postgres \
			   -p 50053:50053 character-service:1.0.0