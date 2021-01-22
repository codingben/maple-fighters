build:
	docker build -t game-service:1.0.0 .

run:
	docker run -e URL=ws://0.0.0.0:50051 -p 50051:50051 game-service:1.0.0