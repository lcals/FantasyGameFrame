#!/bin/bash
echo "mac_open_static_file_server.sh"
echo "pwd  $(pwd)"
cd ./../files_server || exit 1
chmod u+x start_file_server.sh
./start_file_server.sh