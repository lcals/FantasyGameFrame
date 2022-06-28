using System.Collections.Generic;
using Cysharp.Text;
using Fantasy.Frame;
using Fantasy.Logic.Interface;
using Microsoft.Extensions.Logging;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using ZLogger;

namespace Fantasy.Logic.Achieve
{
    public class FantasyUIModule: AModule, IFantasyUIModule
    {
        private const string UIPath = "Assets/Asset/UI/Prefabs/{0}.prefab";

        private readonly Dictionary<string, AFantasyBaseUI> _ui = new();
        private readonly Dictionary<string,  AsyncOperationHandle<GameObject>> _loadAssetAsyncDic = new();
        private readonly Transform _uIRoot;
        private IFantasyLogModule _fantasyLogModule;
        private ILogger<FantasyUIModule> _logger;
       

        public FantasyUIModule(PluginManager pluginManager, bool isUpdate) : base(pluginManager, isUpdate)
        {
            var uiRoot = GameObject.FindWithTag("UICamera");
            Object.DontDestroyOnLoad(uiRoot);
            _uIRoot = uiRoot.transform.Find("UICanvas");
            var mainCamera = GameObject.FindWithTag("MainCamera");
            Object.DontDestroyOnLoad(mainCamera);
        }

        public override void Awake()
        {
            _fantasyLogModule = PluginManager.FindModule<IFantasyLogModule>();
            Debug.Assert(_fantasyLogModule != null, nameof(_fantasyLogModule) + " != null");
            _logger = _fantasyLogModule.GetLogger<FantasyUIModule>();
            _logger.ZLogDebug("{0}   Awake", nameof(FantasyUIModule));
        }

        public override void Init()
        {
            
        }

        public override void AfterInit()
        {
           
        }

        public override void Execute()
        {
        }

        public override void BeforeShut()
        {
        }

        public override void Shut()
        {
        }

        public void OpenUI<T>() where T : AFantasyBaseUI
        {
          
            var uiName = typeof(T).ToString();
            _logger.ZLogDebug("OpenUI  {0}",uiName);
            if (_ui.TryGetValue(uiName, out var baseUI))
            {
                if (!baseUI.IsOpen)
                {
                    baseUI.Display();
                }
            }
            else
            {
                
                //Fantasy.UI.UIXXX
                var assetName = uiName.Replace("Fantasy.UI.UI","");
                var loadAssetAsync =Addressables.LoadAssetAsync<GameObject>(ZString.Format(UIPath, assetName));
                _loadAssetAsyncDic.Add(uiName, loadAssetAsync);
                loadAssetAsync.Completed+= o =>
                {
                    var uiObj  =Object.Instantiate(o.Result, _uIRoot, true);
                    uiObj.GetComponent<RectTransform>().Init();
                    var openUI = uiObj.AddComponent<T>();
                    openUI.Display();
                    _ui.Add(uiName,openUI);
                };
                
            }
        }
        
        public void CloseUI<T>() where T : AFantasyBaseUI
        {
            var uiName = typeof(T).ToString();
            _logger.ZLogDebug("Close  {0}",uiName);
            if (_ui.TryGetValue(uiName, out var baseUI))
            {
                baseUI.Close();
                Object.Destroy(baseUI.gameObject);
                _ui.Remove(uiName);
            }
            if (!_loadAssetAsyncDic.TryGetValue(uiName, out var asyncOperationHandle)) return;
            Addressables.Release(asyncOperationHandle);
            _loadAssetAsyncDic.Remove(uiName);
        }

        public T GetUI<T>() where T : AFantasyBaseUI
        {
            var uiName = typeof(T).ToString();
            if (!_ui.TryGetValue(uiName, out var baseUI)) return null;
            return baseUI as T;
        }
    }
}