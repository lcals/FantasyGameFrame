#!/bin/bash
echo "mac_close_static_file_server.sh"
echo "pwd  $(pwd)"
cd ./../files_server || exit 1
chmod u+x close_file_server.sh
./close_file_server.sh