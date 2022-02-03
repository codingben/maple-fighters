pull:
	docker pull maplefighters/gameprovider-service

build:
	docker build -t gameprovider-service .

run:
	docker run -e RUST_LOG=info \
			   -e RUST_BACKTRACE=1 \
			   -e IP_ADDRESS=0.0.0.0:50052 \
			   -e GAME_SERVICES=[{\"name\":\"Europe\",\"protocol\":\"ws\",\"url\":\"localhost/game/\"}] \
			   -p 50052:50052 gameprovider-service