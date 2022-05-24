using System.IO;
using System.Linq;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Fantasy.Logic.Editor
{
    public class FantasyBuildWithReport:UnityEditor.Editor,IPreprocessBuildWithReport,IPostprocessBuildWithReport
    {
        public int callbackOrder { get; }
        
        public void OnPostprocessBuild(BuildReport report)
        {
            var sourcePath = FantasyAssetModuleEditor.AssetDirectory;
            var targetPath = Application.streamingAssetsPath;
            var oldFiles = Directory.GetFiles(targetPath, "*.*", SearchOption.AllDirectories).ToList();
            //创建所有新目录
            foreach (var dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }
            //复制所有文件 & 保持文件名和路径一致
            foreach (var newPath in Directory.GetFiles(sourcePath, "*.*",SearchOption.AllDirectories))
            {
                var newFilePath = newPath.Replace(sourcePath, targetPath);
                File.Copy(newPath, newFilePath, true);
                oldFiles.Remove(newFilePath);
            }
            
            foreach (var file in oldFiles)
            {
                File.Delete(file);
            }
        }

        public void OnPreprocessBuild(BuildReport report)
        {
           
        }
    }
}