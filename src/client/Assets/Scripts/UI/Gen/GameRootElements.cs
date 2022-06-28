using AnKuchen.Map;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Fantasy.UI.Gen
{
    public class GameRootElements : IMappedObject
    {
        public IMapper Mapper { get; private set; }
        public GameObject Root { get; private set; }
        public Button StartGameButton { get; private set; }
        public TextMeshProUGUI StartGameButtonButtonText { get; private set; }
        public TextMeshProUGUI VersionTitle { get; private set; }
        public Button UpdateGameButton { get; private set; }
        public TextMeshProUGUI UpdateGameButtonButtonText { get; private set; }

        public GameRootElements() { }
        public GameRootElements(IMapper mapper) { Initialize(mapper); }

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

}