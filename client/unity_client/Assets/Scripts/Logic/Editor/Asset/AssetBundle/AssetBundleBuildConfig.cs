using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fantasy.Logic.Editor
{
    [Serializable]
    public class AssetBundleFilter
    {
        public bool Valid = true;
        public string Path = string.Empty;
        public string Filter = "t:Prefab";
    }

    public class AssetBundleBuildConfig : ScriptableObject
    {
        public List<AssetBundleFilter> Filters = new();
    }
}