#!/bin/bash
echo "start_web_server.sh"
cd tools || exit 1
/usr/local/share/dotnet/dotnet static_file_server.dll --webroot="./../files" urls="http://127.0.0.1:9191"