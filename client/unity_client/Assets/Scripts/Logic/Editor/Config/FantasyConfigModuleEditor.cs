using UnityEditor;
using UnityEngine;

namespace Fantasy.Logic.Editor
{
    public static class FantasyConfigModuleEditor 
    {
        [MenuItem("FantasyTools/GenerateDataStruct")]
        public static void GenerateDataStruct()
        {
#if UNITY_EDITOR_OSX
            var shell = $"{Application.dataPath}/Scripts/Logic/Editor/Config/Tools/mac_gen.sh";
            System.Diagnostics.Process.Start("/bin/bash", shell);
            Debug.Log($"Shall_Open : {shell}");  
#endif
          
        }

       
       
    }
}