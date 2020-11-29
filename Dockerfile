FROM nginx:alpine

WORKDIR /build/webgl
COPY webgl/ .

COPY ./nginx/nginx.conf /etc/nginx/conf.d/default.conf