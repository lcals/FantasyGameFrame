using AnKuchen.Map;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using Fantasy.Logic;
using Fantasy.Logic.Achieve;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Fantasy.UI
{
    public class GameRootUiElements : IMappedObject
    {
        public IMapper Mapper { get; private set; }
        public GameObject Root { get; private set; }
        public Button StartGameButton { get; private set; }
        public TextMeshProUGUI StartGameButtonButtonText { get; private set; }
        public TextMeshProUGUI VersionTitle { get; private set; }
        public Button UpdateGameButton { get; private set; }
        public TextMeshProUGUI UpdateGameButtonButtonText { get; private set; }

        public GameRootUiElements() { }
        public GameRootUiElements(IMapper mapper) { Initialize(mapper); }

        public void Initialize(IMapper mapper)
        {
            Mapper = mapper;
            Root = mapper.Get();
            StartGameButton = mapper.Get<Button>("StartGameButton");
            StartGameButtonButtonText = mapper.Get<TextMeshProUGUI>("StartGameButton/ButtonText");
            VersionTitle = mapper.Get<TextMeshProUGUI>("VersionTitle");
            UpdateGameButton = mapper.Get<Button>("UpdateGameButton");
            UpdateGameButtonButtonText = mapper.Get<TextMeshProUGUI>("UpdateGameButton/ButtonText");
        }
    }


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
            if (pluginManager.FindModule<IFantasyAssetModule>() is not IFantasyAssetModule iAsset)
            {
                return;
            }
            await UniTask.WaitUntil(() => iAsset.GetInitSuccessful());
            var oldVersion = iAsset.GetOLdVersionInfoT();
            if (oldVersion.Update)
            {
                await UniTask.WaitUntil(() => iAsset.GetUpdateSuccessful());
                var newVersion = iAsset.GetNewVersionInfoT();
                if (oldVersion.TotalVersion==newVersion.TotalVersion)
                {
                    gameRootUiElements.VersionTitle.text = oldVersion.TotalVersion.ToString();
                    return;
                }
                gameRootUiElements.VersionTitle.text = ZString.Format("{0} -> {1}",oldVersion.TotalVersion.ToString(),
                    newVersion.TotalVersion.ToString());
                gameRootUiElements.UpdateGameButton.gameObject.SetActive(true);
                await gameRootUiElements.UpdateGameButton.OnClickAsync();
                iAsset.StartUpdate();
                gameRootUiElements.UpdateGameButton.gameObject.SetActive(false);
            }
            else
            {
                gameRootUiElements.VersionTitle.text = oldVersion.TotalVersion.ToString();
            }
        }
    }
}