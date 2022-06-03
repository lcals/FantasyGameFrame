using AnKuchen.Map;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using Fantasy.Logic;
using Fantasy.Logic.Interface;
using Fantasy.UI.Gen;
using UnityEngine;

namespace Fantasy.UI
{
    public class UIGameRoot : MonoBehaviour
    {
        private void Awake()
        {
            GameRootFlow().Forget();
        }

        private async UniTask GameRootFlow()
        {
            var gameRootUICache = GetComponent<UICache>();
            var gameRootUiElements = new GameRootUiElements(gameRootUICache)
            {
                VersionTitle =
                {
                    text = ""
                }
            };
            gameRootUiElements.UpdateGameButton.gameObject.SetActive(false);
            await UniTask.WaitUntil(() => GameRoot.Successful);
            var pluginManager = GameRoot.GetPluginManager();
            if (pluginManager.FindModule<IFantasyVersionModule>() is not IFantasyVersionModule fantasyVersionModule)
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
                gameRootUiElements.UpdateGameButton.gameObject.SetActive(false);
            }
            else
            {
                gameRootUiElements.VersionTitle.text = oldVersion.TotalVersion.ToString();
            }
        }
    }
}