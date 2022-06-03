using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Fantasy.Logic.Editor;
using Fantasy.Logic.Interface;
using Fantasy.VersionInfo;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Logic.Editor.Version
{
    public static class FantasyVersionModuleEditor
    {
        [MenuItem("FantasyTools/Version/GenerateDataStruct")]
        public static void GenerateDataStruct()
        {
#if UNITY_EDITOR_OSX
            var shell = $"{Application.dataPath}/Scripts/Logic/Editor/Version/Tools/mac_gen.sh";
            Process.Start("/bin/bash", shell);
            Debug.Log($"Shall_Open : {shell}");
#endif
        }

        [MenuItem("FantasyTools/Version/GenerateDefaultVersion")]
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
            if (!Directory.Exists(FantasyAssetPathEditor.LocalResourceDirectory))
                Directory.CreateDirectory(FantasyAssetPathEditor.LocalResourceDirectory);

            File.WriteAllText(
                $"{FantasyAssetPathEditor.LocalResourceDirectory}/{FantasyVersionModule.VersionInfoName.Replace(".json", "").Replace(".bin", "")}.json",
                versionInfoT.SerializeToJson());
            File.WriteAllBytes(
                $"{FantasyAssetPathEditor.LocalResourceDirectory}/{FantasyVersionModule.VersionInfoName.Replace(".json", "").Replace(".bin", "")}.bin",
                versionInfoT.SerializeToBinary());
        }

        public static void AddDataVersion()
        {
            if (!Directory.Exists(FantasyAssetPathEditor.LocalResourceDirectory)) GenerateDefaultVersion();

            var jsonPath =
                $"{FantasyAssetPathEditor.LocalResourceDirectory}/{FantasyVersionModule.VersionInfoName.Replace(".json", "").Replace(".bin", "")}.json";
            var versionInfoT = VersionInfoT.DeserializeFromJson(File.ReadAllText(jsonPath));
            versionInfoT.DataVersion++;
            versionInfoT.TotalVersion++;

            File.WriteAllText(
                $"{FantasyAssetPathEditor.LocalResourceDirectory}/{FantasyVersionModule.VersionInfoName.Replace(".json", "").Replace(".bin", "")}.json",
                versionInfoT.SerializeToJson());
            File.WriteAllBytes(
                $"{FantasyAssetPathEditor.LocalResourceDirectory}/{FantasyVersionModule.VersionInfoName.Replace(".json", "").Replace(".bin", "")}.bin",
                versionInfoT.SerializeToBinary());
        }
    }
}