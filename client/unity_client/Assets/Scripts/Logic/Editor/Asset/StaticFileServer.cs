using System.Threading;
using UnityEditor;
using UnityEngine;

namespace Fantasy.Logic.Editor
{
    public static class StaticFileServer
    {
        [MenuItem("FantasyTools/Asset/OpenStaticFileServer")]
        public static void OpenStaticFileServer()
        {
            var shell = $"{Application.dataPath}/Scripts/Logic/Editor/Asset/Tools/mac_open_static_file_server.sh";
            var thread=new Thread(() =>
            {
#if UNITY_EDITOR_OSX
                try
                {
                    var proc = new System.Diagnostics.Process();
                    proc.StartInfo.FileName ="/bin/zsh";
                    proc.StartInfo.Arguments = shell;
                    proc.StartInfo.CreateNoWindow = false;
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.RedirectStandardError = true;
                    proc.StartInfo.RedirectStandardOutput = true;
                    Debug.Log($"Shall_Open : {shell}");
                    proc.Start();
                    Debug.Log(proc.StandardOutput.ReadToEnd());
                    Debug.Log(proc.StandardError.ReadToEnd());
                    proc.WaitForExit();               
                }
                catch
                {
                    // ignored
                }
#endif
            });//创建线程
            thread.Start();  

        }
        [MenuItem("FantasyTools/Asset/CloseStaticFileServer")]
        public static void CloseStaticFileServer()
        {
#if UNITY_EDITOR_OSX
            var shell = $"{Application.dataPath}/Scripts/Logic/Editor/Asset/Tools/mac_close_static_file_server.sh";
            try
            {
                var proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName ="/bin/zsh";
                proc.StartInfo.Arguments = shell;
                proc.StartInfo.CreateNoWindow = false;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.RedirectStandardOutput = true;
                Debug.Log($"Shall_Open : {shell}");
                proc.Start();
                Debug.Log(proc.StandardOutput.ReadToEnd());
                Debug.Log(proc.StandardError.ReadToEnd());
                proc.WaitForExit(); 
               
            }
            catch
            {
                // ignored
            }
#endif
        }
    }
}