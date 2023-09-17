#!/bin/sh

# Removes the cloudflare-ips.conf file if REMOVE_CF_IPS is set to "true".
# This is necessary to allow all client connections, otherwise only
# Cloudflare IPs would be allowed (that used in Production only).
remove_cf_ips() {
  if [ "$REMOVE_CF_IPS" == "true" ]; then
    rm -f /var/www-allow/cloudflare-ips.conf
  fi
}

# Updates the REACT_APP_ENV variable in the React.js application's config.js file.
# It replaces the placeholder __REACT_APP_ENV__ with the actual
# environment variable before the app starts.
set_config() {
  sed -i "s|__REACT_APP_ENV__|${REACT_APP_ENV}|g" /usr/share/nginx/html/config.js
}

# Starts the nginx server in foreground mode.
# The 'daemon off;' ensures nginx runs in the foreground, useful for running inside
# Docker containers.
start_nginx() {
  exec nginx -g 'daemon off;'
}

# 1. Optionally removes Cloudflare IPs.
# 2. Sets the React.js app environment.
# 3. Starts the nginx server.
main() {
  remove_cf_ips
  set_config
  start_nginx
}

main
