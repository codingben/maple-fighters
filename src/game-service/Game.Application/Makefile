build:
	docker build -t game-service .

run:
	docker run -p 50051:50051 game-service -e \
		URL=ws://0.0.0.0:50051 \
		FLECK_LOG=Info \
		IM_LOG=Debug \
		GAME_LOG=Debug \
		CONFIG_SOURCE=v2.0 \
		MAX_CONNECTIONS=100