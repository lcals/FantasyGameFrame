using System;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Fantasy.Logic.Editor
{
    public class SetAssetBundle : EditorWindow
    {
        private const string SavePath = "Assets/Scripts/Logic/Editor/CacheData/AssetBundleBuildConfig.asset";
        private AssetBundleBuildConfig _config;
        private ReorderableList _list;
        private Vector2 _scrollPosition = Vector2.zero;

        [MenuItem("FantasyTools/Asset/SetAssetBundle")]
        public static void Open()
        {
            GetWindow<SetAssetBundle>("Set Asset Bundle", true);
        }

        public static bool SetBundleName()
        {
            var config = AssetDatabase.LoadAssetAtPath<AssetBundleBuildConfig>(SavePath);
            if (config == null)
                return false;
            foreach (var assetBundleFilter in config.Filters.Where(f => f.Valid))
            {
                var guids = AssetDatabase.FindAssets(assetBundleFilter.Filter, new[] {assetBundleFilter.Path});
                foreach (var guid in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var assetImporter = AssetImporter.GetAtPath(path);
                    var assetBundleName = path.ToLower().Replace("assets/", "");
                    if (assetImporter.assetBundleName == assetBundleName) continue;
                    assetImporter.assetBundleName = assetBundleName;
                    assetImporter.userData = DateTime.UtcNow.ToString("u");
                    assetImporter.SaveAndReimport();
                }
            }

            return true;
        }


        private void OnGUI()
        {
            if (_config == null) InitConfig();

            if (_list == null) InitFilterListDrawer();

            //tool bar
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                if (GUILayout.Button("Add", EditorStyles.toolbarButton)) Add();

                if (GUILayout.Button("Save", EditorStyles.toolbarButton)) Save();

                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
            //context
            GUILayout.BeginVertical();
            {
                _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
                {
                    _list.DoLayoutList();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
            //set dirty
            if (GUI.changed)
                EditorUtility.SetDirty(_config);
        }


        private string SelectFolder()
        {
            var dataPath = Application.dataPath;
            var selectedPath = EditorUtility.OpenFolderPanel("Path", dataPath, "");
            if (string.IsNullOrEmpty(selectedPath)) return null;
            if (selectedPath.StartsWith(dataPath)) return $"Assets/{selectedPath[(dataPath.Length + 1)..]}";

            ShowNotification(new GUIContent("Cannot be outside the assets directory!"));
            return null;
        }

        private void InitConfig()
        {
            _config = AssetDatabase.LoadAssetAtPath<AssetBundleBuildConfig>(SavePath);
            if (_config == null) _config = CreateInstance<AssetBundleBuildConfig>();
        }

        private void InitFilterListDrawer()
        {
            _list = new ReorderableList(_config.Filters, typeof(AssetBundleFilter))
            {
                drawHeaderCallback = rect => { EditorGUI.LabelField(rect, "Asset Filter"); },
                drawElementCallback = OnListElementGUI,
                draggable = true,
                elementHeight = 22,
                onAddCallback = _ => Add()
            };
        }

        private void OnListElementGUI(Rect rect, int index, bool isActive, bool isFocused)
        {
            const float gap = 5;
            var filter = _config.Filters[index];
            rect.y++;
            var r = rect;
            r.width = 16;
            r.height = 18;
            filter.Valid = GUI.Toggle(r, filter.Valid, GUIContent.none);
            r.xMin = r.xMax + gap;
            r.xMax = rect.xMax - 300;
            GUI.enabled = false;
            filter.Path = GUI.TextField(r, filter.Path);
            GUI.enabled = true;
            r.xMin = r.xMax + gap;
            r.width = 50;
            if (GUI.Button(r, "Select"))
            {
                var path = SelectFolder();
                if (path != null)
                    filter.Path = path;
            }

            r.xMin = r.xMax + gap;
            r.xMax = rect.xMax;
            filter.Filter = GUI.TextField(r, filter.Filter);
        }


        private void Add()
        {
            var path = SelectFolder();
            if (string.IsNullOrEmpty(path)) return;
            var filter = new AssetBundleFilter
            {
                Path = path
            };
            _config.Filters.Add(filter);
        }


        private void Save()
        {
            if (AssetDatabase.LoadAssetAtPath<AssetBundleBuildConfig>(SavePath) == null)
                AssetDatabase.CreateAsset(_config, SavePath);
            else
                EditorUtility.SetDirty(_config);

            SetBundleName();
            AssetDatabase.SaveAssets();
        }
    }
}