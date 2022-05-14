#!/bin/bash
echo "mac_gen.sh"
cd Assets/Scripts/Logic/Editor/Config/Tools || exit 1
./flatc  -o ./../../../Runtime/Config --csharp --cs-gen-json-serializer  --gen-onefile --gen-object-api ./../Fbs/monster.fbs