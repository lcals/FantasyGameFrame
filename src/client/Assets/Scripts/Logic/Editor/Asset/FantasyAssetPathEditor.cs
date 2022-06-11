using Fantasy.Logic.Achieve;

namespace Fantasy.Logic.Editor
{
    public static class FantasyAssetPathEditor
    {
        public static string LocalResourceDirectory
        {
            get
            {
                FantasyAssetPath.Init();
                return FantasyAssetPath.LocalResourceDirectory;
            }
        }
    }
}