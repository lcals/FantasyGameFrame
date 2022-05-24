using Fantasy.VersionInfo;

namespace Fantasy.Logic.Achieve
{
    public interface IFantasyAssetModule
    {
        public string GetLocalResourceDirectory();
        public string GetCacheResourceDirectory();
        bool GetInitSuccessful();
        bool GetUpdateSuccessful();
        VersionInfoT GetOLdVersionInfoT();
        VersionInfoT GetNewVersionInfoT();
        void StartUpdate();
    }
}