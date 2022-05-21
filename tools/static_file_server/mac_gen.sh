#!/bin/bash
echo "mac_gen.sh"
dotnet publish -c Release -r osx.12-arm64  -o ./../../client/web_server/tools --self-contained false