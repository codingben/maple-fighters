FROM node:16.9.0-alpine as builder
WORKDIR /app
COPY package*.json /app/
RUN npm install --only=production
COPY ./ /app/
RUN npm run build

FROM nginx:1.20.1-alpine
COPY --from=builder /app/nginx.conf /etc/nginx/nginx.conf
COPY --from=builder /app/cloudflare-ips.conf /var/www-allow/cloudflare-ips.conf
COPY --from=builder /app/build /usr/share/nginx/html
COPY --from=builder /app/entrypoint.sh /entrypoint.sh
ENTRYPOINT ["/entrypoint.sh"]