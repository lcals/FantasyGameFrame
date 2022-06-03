using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Fantasy.Config;
using Logic.Editor.Version;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Fantasy.Logic.Editor
{
    public static class FantasyConfigModuleEditor
    {
        [MenuItem("FantasyTools/Config/GenerateDataStruct")]
        public static void GenerateDataStruct()
        {
#if UNITY_EDITOR_OSX
            var shell = $"{Application.dataPath}/Scripts/Logic/Editor/Config/Tools/mac_gen.sh";
            Process.Start("/bin/bash", shell);
            Debug.Log($"Shall_Open : {shell}");
#endif
        }

        [MenuItem("FantasyTools/Config/UpdateCsv")]
        public static void UpdateCsv()
        {
            var items = ReaderRecords<ItemT>(nameof(ItemT));
            if (items.Count == 0) items.Add(new ItemT());
            WriteRecords(items, nameof(ItemT));
            var roles = ReaderRecords<RoleT>(nameof(RoleT));
            if (roles.Count == 0) roles.Add(new RoleT());
            WriteRecords(roles, nameof(RoleT));
        }

        [MenuItem("FantasyTools/Config/GenerateDataBytes")]
        public static void GenerateDataBin()
        {
            var binPath = $"{FantasyAssetPathEditor.LocalResourceDirectory}config/config.bytes";
            UpdateCsv();
            GenerateDataBin(binPath);
            FantasyVersionModuleEditor.AddDataVersion();
        }

        private static void GenerateDataBin(string binPath)
        {
            var dic = Path.GetDirectoryName(binPath);
            if (!Directory.Exists(dic) && !string.IsNullOrEmpty(dic)) Directory.CreateDirectory(dic);
            const string cacheDataPath = "Assets/Scripts/Logic/Editor/CacheData/ConfigRoot.json";
            var version = 0;
            if (File.Exists(cacheDataPath))
                try
                {
                    var configRootT = JsonConvert.DeserializeObject<ConfigRootT>(File.ReadAllText(cacheDataPath));
                    if (configRootT != null) version = configRootT.Version + 1;
                }
                catch (Exception)
                {
                    version = 0;
                }

            var rootT = new ConfigRootT
            {
                Version = version,
                OutputTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                Item = ReaderRecords<ItemT>(nameof(ItemT)),
                Role = ReaderRecords<RoleT>(nameof(RoleT))
            };
            var newJson = rootT.SerializeToJson();
            File.WriteAllText(cacheDataPath, newJson);
            var newBin = rootT.SerializeToBinary();
            File.WriteAllBytes(binPath, newBin);
            AssetDatabase.Refresh();
        }


        private static List<T> ReaderRecords<T>(string path)
        {
            path = $"{Application.dataPath}/../../../config/{path}.csv";
            if (!File.Exists(path)) return new List<T>();
            using var writer = new StreamReader(path);
            using var csv = new CsvReader(writer, CultureInfo.InvariantCulture);
            csv.AddConverter();
            csv.Context.TypeConverterCache.AddConverter<List<string>>(new JsonConverter<List<string>>());
            var records = csv.GetRecords<T>().ToList();
            return records;
        }

        private static void WriteRecords<T>(IEnumerable<T> records, string path)
        {
            path = $"{Application.dataPath}/../../../config/{path}.csv";
            using var writer = new StreamWriter(path);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.AddConverter();
            csv.WriteRecords(records);
        }

        private static void AddConverter(this IReaderRow csv)
        {
            csv.Context.TypeConverterCache.AddConverter<List<int>>(new JsonConverter<List<int>>());
            csv.Context.TypeConverterCache.AddConverter<List<string>>(new JsonConverter<List<string>>());
        }

        private static void AddConverter(this IWriterRow csv)
        {
            csv.Context.TypeConverterCache.AddConverter<List<int>>(new JsonConverter<List<int>>());
            csv.Context.TypeConverterCache.AddConverter<List<string>>(new JsonConverter<List<string>>());
        }

        private class JsonConverter<T> : ITypeConverter
        {
            public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                return JsonConvert.DeserializeObject<T>(text);
            }

            public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
            {
                return JsonConvert.SerializeObject(value);
            }
        }
    }
}