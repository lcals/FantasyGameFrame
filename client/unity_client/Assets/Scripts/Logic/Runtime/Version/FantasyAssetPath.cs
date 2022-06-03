using System;
using UnityEngine;

namespace Fantasy.Logic.Achieve
{
    public static class FantasyAssetPath
    {
        public static string LocalResourceDirectory;
        public static string CacheResourceDirectory;
        
        public  static void Init()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.WindowsEditor:
                    LocalResourceDirectory = $"{Application.streamingAssetsPath}/../AssetDirectory~/LocalResource/";
                    CacheResourceDirectory = $"{Application.streamingAssetsPath}/../AssetDirectory~/CacheResource/";
                    break;
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.Android:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.OSXPlayer:
                    LocalResourceDirectory = $"{Application.streamingAssetsPath}/";
                    CacheResourceDirectory = $"{Application.persistentDataPath}/";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}