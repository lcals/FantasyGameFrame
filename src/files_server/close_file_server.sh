#!/bin/bash
echo "close_file_server.sh"
kill -9 $(ps -ef|grep 'static_file_server' |awk '$0 !~/grep/ {print $2}' |tr -s '\n' ' ')
