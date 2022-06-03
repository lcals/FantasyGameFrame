using Fantasy.VersionInfo;

namespace Fantasy.Logic.Interface
{
    public interface IFantasyVersionModule
    {
        bool GetInitSuccessful();
        VersionInfoT GetOldVersionInfoT();
        bool GetUpdateSuccessful();
        VersionInfoT GetNewVersionInfoT();
        void StartUpdate();
     
    }
}