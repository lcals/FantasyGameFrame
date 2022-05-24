using System;
using System.Globalization;
using System.IO;
using Fantasy.Logic.Achieve;
using Fantasy.VersionInfo;
using UnityEditor;
using UnityEngine;

namespace Fantasy.Logic.Editor
{
    public class FantasyAssetModuleEditor
    {
        public static readonly string AssetDirectory = $"{Application.streamingAssetsPath}/../AssetDirectory~";

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
        }
     
        public static void AddDataVersion()
        {
            if (!Directory.Exists(AssetDirectory))
            {
                GenerateDefaultVersion();
            }
            var jsonPath =
                $"{AssetDirectory}/{FantasyAssetModule.VersionInfoName.Replace(".json", "").Replace(".bin", "")}.json";
            var versionInfoT = VersionInfoT.DeserializeFromJson(File.ReadAllText(jsonPath));
            versionInfoT.DataVersion++;
            versionInfoT.TotalVersion++;
            File.WriteAllText(
                $"{AssetDirectory}/{FantasyAssetModule.VersionInfoName.Replace(".json", "").Replace(".bin", "")}.json",
                versionInfoT.SerializeToJson());
            File.WriteAllBytes(
                $"{AssetDirectory}/{FantasyAssetModule.VersionInfoName.Replace(".json", "").Replace(".bin", "")}.bin",
                versionInfoT.SerializeToBinary());
        }
    }
}