using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Fantasy.Logic.Achieve;
using Fantasy.VersionInfo;
using UnityEditor;
using UnityEngine;

namespace Fantasy.Logic.Editor
{
    public class FantasyAssetModuleEditor
    {
        private static readonly string AssetDirectory = $"{Application.streamingAssetsPath}/../AssetDirectory~";

        [MenuItem("FantasyTools/Asset/GenerateDataStruct")]
        public static void GenerateDataStruct()
        {
#if UNITY_EDITOR_OSX
            var shell = $"{Application.dataPath}/Scripts/Logic/Editor/Asset/Tools/mac_gen.sh";
            System.Diagnostics.Process.Start("/bin/bash", shell);
            Debug.Log($"Shall_Open : {shell}");
#endif
        }
        [MenuItem("FantasyTools/Asset/GenerateDefaultVersion")]
        public static void GenerateDefaultVersion()
        {
            var versionInfoT = new VersionInfoT
            {
                Url = "http://127.0.0.1:9191/",
                Update = true,
                OutputTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                ServerIp = "127.0.0.1:91",
                DataVersion = 0,
                AssetVersion = 0,
                TotalVersion = 0
            };
            if (!Directory.Exists(AssetDirectory))
            {
                Directory.CreateDirectory(AssetDirectory);
            }
            File.WriteAllText(
                $"{AssetDirectory}/{FantasyAssetModule.VersionInfoName.Replace(".json", "").Replace(".bin", "")}.json",
                versionInfoT.SerializeToJson());
            File.WriteAllBytes(
                $"{AssetDirectory}/{FantasyAssetModule.VersionInfoName.Replace(".json", "").Replace(".bin", "")}.bin",
                versionInfoT.SerializeToBinary());
            AssetDatabase.Refresh();
        }
        [MenuItem("FantasyTools/Asset/CopyFilesToFileServer")]
        public static void CopyFilesToFileServer()
        {
                var sourcePath = AssetDirectory;
                var targetPath = $"{Application.dataPath}/../../web_server/files/";
                var oldFiles = Directory.GetFiles(targetPath, "*.*", SearchOption.AllDirectories).ToList();
            
                //创建所有新目录
                foreach (var dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
                }
                //复制所有文件 & 保持文件名和路径一致
                foreach (var newPath in Directory.GetFiles(sourcePath, "*.*",SearchOption.AllDirectories))
                {
                    var newFilePath = newPath.Replace(sourcePath, targetPath);
                    File.Copy(newPath, newFilePath, true);
                    oldFiles.Remove(newFilePath);
                }
                foreach (var file in oldFiles)
                {
                    File.Delete(file);
                }
            
        }
    }
}