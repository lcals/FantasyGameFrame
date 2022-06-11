#!/bin/bash
echo "mac_gen.sh"
cd Assets/Scripts/Logic/Editor/Config/Tools || exit 1
dir="./../../../Runtime/Config/Gen/"
for dir_del in $dir/* ; do
 if [ -d $dir_del ]; then
  rm -rf $dir_del
 fi
done
./flatc  --csharp   --csharp --cs-gen-json-serializer  --gen-object-api    -o ./../../../Runtime/Config/Gen  ./../Fbs/ConfigRoot.fbs ./../Fbs/Role.fbs ./../Fbs/Item.fbs