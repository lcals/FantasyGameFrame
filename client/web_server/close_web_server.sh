#!/bin/bash
echo "close_web_server.sh"
# shellcheck disable=SC2046
kill -9 $(ps -ef|grep 'static_file_server' |awk '$0 !~/grep/ {print $2}' |tr -s '\n' ' ')
