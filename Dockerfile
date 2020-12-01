FROM nginx:alpine

COPY ./build/webgl/ ./build/webgl/
COPY ./src/nginx/nginx.conf /etc/nginx/conf.d/default.conf