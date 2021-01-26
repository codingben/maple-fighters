build:
	docker build -t auth-service:1.0.0 .

run:
	docker run -e DATABASE_URL=mongodb://mongo:27017/maple_fighters -p 50050:50050 auth-service:1.0.0