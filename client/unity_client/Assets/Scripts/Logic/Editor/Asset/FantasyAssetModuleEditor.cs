using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Fantasy.Logic.Editor
{
    public static class FantasyAssetModuleEditor
    {
        [MenuItem("FantasyTools/Asset/GenerateDataStruct")]
        public static void GenerateDataStruct()
        {
#if UNITY_EDITOR_OSX
            var shell = $"{Application.dataPath}/Scripts/Logic/Editor/Asset/Tools/mac_gen.sh";
            Process.Start("/bin/bash", shell);
            Debug.Log($"Shall_Open : {shell}");
#endif
        }
    }
}