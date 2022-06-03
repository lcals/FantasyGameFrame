using System.IO;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace Fantasy.Logic.Editor
{
    public static class StaticFileServer
    {
        [MenuItem("FantasyTools/Files/OpenStaticFileServer")]
        public static void OpenStaticFileServer()
        {
            var shell = $"{Application.dataPath}/Scripts/Logic/Editor/Asset/Tools/mac_open_static_file_server.sh";
            var thread = new Thread(() =>
            {
#if UNITY_EDITOR_OSX
                try
                {
                    var proc = new System.Diagnostics.Process();
                    proc.StartInfo.FileName = "/bin/zsh";
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
            }); //创建线程
            thread.Start();
        }

        [MenuItem("FantasyTools/Files/CloseStaticFileServer")]
        public static void CloseStaticFileServer()
        {
#if UNITY_EDITOR_OSX
            var shell = $"{Application.dataPath}/Scripts/Logic/Editor/Asset/Tools/mac_close_static_file_server.sh";
            try
            {
                var proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = "/bin/zsh";
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

        [MenuItem("FantasyTools/Files/CopyFilesToFileServer")]
        public static void CopyFilesToFileServer()
        {
            var sourcePath = FantasyAssetPathEditor.LocalResourceDirectory;
            var targetPath = $"{Application.dataPath}/../../web_server/files/";
            var oldFiles = Directory.GetFiles(targetPath, "*.*", SearchOption.AllDirectories).ToList();
            //创建所有新目录
            foreach (var dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            //复制所有文件 & 保持文件名和路径一致
            foreach (var newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                var newFilePath = newPath.Replace(sourcePath, targetPath);
                File.Copy(newPath, newFilePath, true);
                oldFiles.Remove(newFilePath);
            }

            foreach (var file in oldFiles) File.Delete(file);
            Debug.Log("CopyFilesToFileServer end");
        }
    }
}