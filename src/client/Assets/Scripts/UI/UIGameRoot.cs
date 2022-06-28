using Cysharp.Text;
using Cysharp.Threading.Tasks;
using Fantasy.Logic;
using Fantasy.Logic.Achieve;
using Fantasy.Logic.Interface;
using Fantasy.UI.Gen;

namespace Fantasy.UI
{
    public class UIGameRoot : AFantasyBaseUI
    {
       

        private void OnEnable()
        {
            GameRootFlow().Forget();
        }
        private async UniTask GameRootFlow()
        {
         
            var gameRootUiElements = new GameRootElements(GameRootUICache)
            {
                VersionTitle =
                {
                    text = ""
                }
            };
            gameRootUiElements.UpdateGameButton.gameObject.SetActive(false);
            await UniTask.WaitUntil(() => GameRoot.Successful);
            var pluginManager = GameRoot.GetPluginManager();
            if (pluginManager.FindModule<IFantasyVersionModule>() is not { } fantasyVersionModule)
                return;
            await UniTask.WaitUntil(() => fantasyVersionModule.GetInitSuccessful());
            var oldVersion = fantasyVersionModule.GetOldVersionInfoT();
            if (oldVersion.Update)
            {
                await UniTask.WaitUntil(() => fantasyVersionModule.GetUpdateSuccessful());
                var newVersion = fantasyVersionModule.GetNewVersionInfoT();
                if (oldVersion.TotalVersion == newVersion.TotalVersion)
                {
                    gameRootUiElements.VersionTitle.text = oldVersion.TotalVersion.ToString();
                    return;
                }
                gameRootUiElements.VersionTitle.text = ZString.Format("{0} -> {1}", oldVersion.TotalVersion.ToString(),
                    newVersion.TotalVersion.ToString());
                gameRootUiElements.UpdateGameButton.gameObject.SetActive(true);
                await gameRootUiElements.UpdateGameButton.OnClickAsync();
                fantasyVersionModule.StartUpdate();
                await UniTask.WaitUntil(() => fantasyVersionModule.GetGameUpdateSuccessful());
                gameRootUiElements.UpdateGameButton.gameObject.SetActive(false);
                await gameRootUiElements.StartGameButton.OnClickAsync();
                if (pluginManager.FindModule<IFantasyUIModule>() is not { } fantasyUIModule)
                    return;
                fantasyUIModule.OpenUI<UILogin>();
                await UniTask.WaitUntil(() => fantasyUIModule.GetUI<UILogin>()!=null);
                gameRootUiElements.Root.SetActive(false);
                
            }
            else
            {
                gameRootUiElements.VersionTitle.text = oldVersion.TotalVersion.ToString();
            }
        }
    }
}