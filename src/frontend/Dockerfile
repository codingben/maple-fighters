FROM node:16.9.0-alpine as builder
WORKDIR /app
COPY package*.json /app/
RUN npm install --only=production
COPY ./ /app/
ARG ENVIRONMENT
ENV REACT_APP_ENV ${ENVIRONMENT}
RUN npm run build

FROM nginx:1.20.1-alpine
ARG NGINX_CONF_FILE
COPY --from=builder /app/${NGINX_CONF_FILE} /etc/nginx/nginx.conf
COPY --from=builder /app/cloudflare-ips.conf /var/www-allow/cloudflare-ips.conf
COPY --from=builder /app/build /usr/share/nginx/html