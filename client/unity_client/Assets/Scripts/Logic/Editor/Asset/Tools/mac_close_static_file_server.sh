#!/bin/bash
echo "mac_close_static_file_server.sh"
echo "pwd  $(pwd)"
cd ./../web_server || exit 1
chmod u+x close_web_server.sh
./close_web_server.sh