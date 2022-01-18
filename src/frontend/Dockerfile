FROM node:16.9.0-alpine as builder
WORKDIR /app
COPY package*.json /app/
RUN npm install --only=production
COPY ./ /app/
RUN npm run build

FROM nginx:1.20.1-alpine
COPY --from=builder /app/build /usr/share/nginx/html