build:
	docker build -t chat-service:1.0.0 .

run:
	docker run -e PORT=50054 \
			   -p 50054:50054 chat-service:1.0.0