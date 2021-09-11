FROM node:16.9.0-alpine
COPY ./ ./
RUN npm ci --only=production
CMD ["npm", "start"]