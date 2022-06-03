using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fantasy.HashCacheInfo;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.Build.Pipeline;
using UnityEngine.Build.Pipeline;

namespace Fantasy.Logic.Editor
{
    public static class BuildAssetBundles
    {
        [MenuItem("FantasyTools/Asset/BuildAssetBundles/Android")]
        public static void BuildAssetBundlesAndroid()
        {
            if (SetAssetBundle.SetBundleName())
                BuildAssetBundle(BuildTarget.Android);
        }

        [MenuItem("FantasyTools/Asset/BuildAssetBundles/iOS")]
        public static void BuildAssetBundlesIOS()
        {
            if (SetAssetBundle.SetBundleName())
                BuildAssetBundle(BuildTarget.iOS);
        }

        [MenuItem("FantasyTools/Asset/BuildAssetBundles/StandaloneWindows64")]
        public static void BuildAssetBundlesStandaloneWindows64()
        {
            if (SetAssetBundle.SetBundleName())
                BuildAssetBundle(BuildTarget.StandaloneWindows64);
        }

        [MenuItem("FantasyTools/Asset/BuildAssetBundles/StandaloneOSX")]
        public static void BuildAssetBundlesStandaloneStandaloneOSX()
        {
            if (SetAssetBundle.SetBundleName())
                BuildAssetBundle(BuildTarget.StandaloneOSX);
        }

        private static void BuildAssetBundle(BuildTarget buildTarget)
        {
            var compatibilityAssetBundleManifest =
                BuildAssetBundle(FantasyAssetPathEditor.LocalResourceDirectory, buildTarget);
            var assetCacheInfoT = new AssetInfoT
            {
                BundleDetails = new List<BundleDetailsT>()
            };
            foreach (var assetBundle in compatibilityAssetBundleManifest.GetAllAssetBundles())
            {
                var bundleDetailsT = new BundleDetailsT
                {
                    FileName = assetBundle,
                    Hash = compatibilityAssetBundleManifest.GetAssetBundleHash(assetBundle).ToString(),
                    Crc = compatibilityAssetBundleManifest.GetAssetBundleCrc(assetBundle).ToString(),
                    Dependencies = compatibilityAssetBundleManifest.GetAllDependencies(assetBundle).ToList()
                };
                assetCacheInfoT.BundleDetails.Add(bundleDetailsT);
            }

            GenVersion(assetCacheInfoT);
        }

        private static CompatibilityAssetBundleManifest BuildAssetBundle(string outputPath, BuildTarget buildTarget)
        {
            var options = BuildAssetBundleOptions.None;
            options |= BuildAssetBundleOptions.ChunkBasedCompression;
            options |= BuildAssetBundleOptions.StrictMode;
            options |= BuildAssetBundleOptions.DisableLoadAssetByFileName;
            options |= BuildAssetBundleOptions.DisableLoadAssetByFileNameWithExtension;
            options |= BuildAssetBundleOptions.AssetBundleStripUnityVersion;
            options |= BuildAssetBundleOptions.ForceRebuildAssetBundle;
            Directory.CreateDirectory(outputPath);
            return CompatibilityBuildPipeline.BuildAssetBundles(outputPath, options, buildTarget);
        }

        private static void GenVersion(AssetInfoT assetInfoT)
        {
            var binPath = $"{FantasyAssetPathEditor.LocalResourceDirectory}asset/asset_info.bytes";
            const string cacheDataPath = "Assets/Scripts/Logic/Editor/CacheData/AssetInfo.json";
            var dic = Path.GetDirectoryName(binPath);
            if (!Directory.Exists(dic) && !string.IsNullOrEmpty(dic)) Directory.CreateDirectory(dic);
            var version = 0;

            if (File.Exists(cacheDataPath))
                try
                {
                    var oldCacheInfoT = JsonConvert.DeserializeObject<AssetInfoT>(File.ReadAllText(cacheDataPath));
                    if (oldCacheInfoT != null) version = oldCacheInfoT.Version + 1;
                }
                catch (Exception)
                {
                    version = 0;
                }

            assetInfoT.Version = version;
            var newJson = assetInfoT.SerializeToJson();
            File.WriteAllText(cacheDataPath, newJson);
            var newBin = assetInfoT.SerializeToBinary();
            File.WriteAllBytes(binPath, newBin);
            AssetDatabase.Refresh();
        }
    }
}