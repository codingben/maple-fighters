build:
	docker build -t auth-service .

run:
	docker run -e ASPNETCORE_ENVIRONMENT=Development \
		   	   -e ASPNETCORE_URLS=http://0.0.0.0:50050 \
		   	   -e DATABASE_URL=mongodb://mongo:27017/maple_fighters \
		       -p 50050:50050 auth-service