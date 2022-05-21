#!/bin/bash
echo "mac_open_static_file_server.sh"
echo "pwd  $(pwd)"
cd ./../web_server || exit 1
chmod u+x start_web_server.sh
./start_web_server.sh