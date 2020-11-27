using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public class FableEditorMenuCreator : EditorWindow
    {
        string _dirPath = "";
        string _menuName = "";
        Scene _newScene;

#if UNITY_EDITOR
        [MenuItem("FableCraft/Create New FableCraft Menu")]
        public static void CreateNewFableMenu()
        {
            FableEditorMenuCreator window = ScriptableObject.CreateInstance<FableEditorMenuCreator>();
            window.position = new Rect(Screen.width / 2 + 250, Screen.height / 2, 250f, 150f);
            window.titleContent = new GUIContent("Menu Creator");
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
            _menuName = EditorGUILayout.TextField("Menu Title", _menuName);
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
            var scriptsDir = "Assets/Scripts/FableCraft/Scenes/";
            var menuDir = $"Assets/{_dirPath}/";

            _menuName = _menuName.Trim();
            if (string.IsNullOrEmpty(_menuName))
            {
                EditorUtility.DisplayDialog("Menu already exists.", "Please specify a valid Menu name to continue.", "Close");
                return;
            }

            var lastFileNumber = Directory.GetFiles(menuDir, "*.meta", SearchOption.AllDirectories).Length + 1;
            FileUtil.CopyFileOrDirectory($"{scriptsDir}New Menu.unity", $"{menuDir}{_menuName} Menu.unity");
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();
            Object obj = AssetDatabase.LoadAssetAtPath<Object>($"{menuDir}{_menuName}.unity");
            Selection.activeObject = obj;

            var original = EditorBuildSettings.scenes;
            var newSettings = new EditorBuildSettingsScene[original.Length + 1];
            System.Array.Copy(original, newSettings, original.Length);
            var sceneToAdd = new EditorBuildSettingsScene($"{menuDir}{_menuName}.unity", true);
            newSettings[newSettings.Length - 1] = sceneToAdd;
            EditorBuildSettings.scenes = newSettings;
        }
#endif
    }
}
