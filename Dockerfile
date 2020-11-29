FROM nginx:alpine

COPY ./build/webgl/ .
COPY ./nginx/nginx.conf /etc/nginx/conf.d/default.conf