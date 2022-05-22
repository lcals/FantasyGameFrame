using Fantasy.VersionInfo;

namespace Fantasy.Logic.Achieve
{
    public interface IFantasyAssetModule
    {
        bool GetInitSuccessful();
        bool GetUpdateSuccessful();
        VersionInfoT GetOLdVersionInfoT();
        VersionInfoT GetNewVersionInfoT();
        void StartUpdate();
    }
}