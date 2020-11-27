using System.IO;
using FableCraft;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public class FableEditorStoryNodeCreator : EditorWindow
    {
        string _dirPath = "";
        string _nodeName = "";
        Scene _newScene;

#if UNITY_EDITOR
        [MenuItem("FableCraft/Create New FableCraft Story Node")]
        public static void CreateNewFableMenu()
        {
            FableEditorStoryNodeCreator window = ScriptableObject.CreateInstance<FableEditorStoryNodeCreator>();
            window.position = new Rect(Screen.width / 2 + 250, Screen.height / 2, 250f, 150f);
            window.titleContent = new GUIContent("Story Node Creator");
            window.Show(true);
        }

        void OnGUI()
        {
            // DIRECTORY UI
            _dirPath = EditorGUILayout.TextField("Creation Folder", _dirPath);
            if (GUILayout.Button("Set"))
            {
                OnClickSaveDirectory();
                GUIUtility.ExitGUI();
            }
            
            // ENTER NAME UI
            _nodeName = EditorGUILayout.TextField("Story Node Title", _nodeName);
            if (GUILayout.Button("Create"))
            {
                OnClickSaveMenu();
                GUIUtility.ExitGUI();
            }
        }

        void OnClickSaveDirectory()
        {
            var menuDir = $"Assets/{_dirPath}/";
            if (AssetDatabase.IsValidFolder($"Assets/{_dirPath}"))
            {
                EditorUtility.DisplayDialog("Folder already exists.", "Please specify a valid Folder name to continue.", "Close");
            }
            else
            {
                var newFolderName = AssetDatabase.CreateFolder("Assets", _dirPath);
                AssetDatabase.Refresh();
                EditorUtility.FocusProjectWindow();
            }
        }

        void OnClickSaveMenu()
        {
            var nodeDir = $"Assets/{_dirPath}/";

            _nodeName = _nodeName.Trim();
            if (string.IsNullOrEmpty(_nodeName))
            {
                EditorUtility.DisplayDialog("Title can't be empty.", "Please specify a valid title to continue.", "Close");
                return;
            }
            
            StoryNode asset = ScriptableObject.CreateInstance<StoryNode>();
            var nodeAssets = AssetDatabase.LoadAllAssetsAtPath($"{nodeDir}{_nodeName}.asset");
            var exists = false;
            
            foreach (var node in nodeAssets)
            {
                if (node.name == _nodeName)
                    exists = true;
                else
                    exists = false;
            }
            
            if (exists)
                EditorUtility.DisplayDialog("Story node already exists.", "Please specify a valid title to continue.", "Close");
            else
            {
                AssetDatabase.CreateAsset(asset, $"{nodeDir}{_nodeName}.asset");
                AssetDatabase.SaveAssets();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = asset;
            }
        }
#endif
    }
}
