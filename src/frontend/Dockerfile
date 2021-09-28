FROM node:16-alpine as builder
WORKDIR /app
COPY package*.json /app/
RUN npm install
COPY ./ /app/
RUN npm run build

FROM nginx:1.20.1-alpine
COPY --from=builder /app/build /usr/share/nginx/html