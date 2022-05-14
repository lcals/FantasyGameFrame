#!/bin/bash
echo "mac_gen.sh"
cd Assets/Scripts/Logic/Editor/Config/Tools || exit 1
./flatc  --csharp   --csharp --cs-gen-json-serializer  --gen-object-api    -o ./../../../Runtime/Config/Gen  ./../Fbs/ConfigRoot.fbs ./../Fbs/Role.fbs ./../Fbs/Item.fbs