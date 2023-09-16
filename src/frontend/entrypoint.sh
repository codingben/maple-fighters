#!/bin/sh

if [ "$REMOVE_CF_IPS" == "true" ]; then
  rm -f /var/www-allow/cloudflare-ips.conf
fi

exec nginx -g 'daemon off;'
