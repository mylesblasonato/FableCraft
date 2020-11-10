using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

namespace FableCraft
{
    public class FableEditorMenuCreator : EditorWindow
    {
        string _menuName = "";
        Scene _newScene;

#if UNITY_EDITOR
        [MenuItem("FableCraft/Create New Fable Menu")]
        public static void CreateNewFableMenu()
        {
            FableEditorMenuCreator window = ScriptableObject.CreateInstance<FableEditorMenuCreator>();
            window.position = new Rect(Screen.width / 2 + 250, Screen.height / 2, 250f, 150f);
            window.titleContent = new GUIContent("Menu Creator");
            window.Show(true);
        }

        void OnGUI()
        {
            _menuName = EditorGUILayout.TextField("Menu Title", _menuName);
            if (GUILayout.Button("Create"))
            {
                OnClickSaveMenu();
                GUIUtility.ExitGUI();
            }
        }

        void OnClickSaveMenu()
        {
            var scriptsDir = "Assets/Scripts/FableCraft/";
            var beatsDir = "Assets/Beats/";

            _menuName = _menuName.Trim();

            if (string.IsNullOrEmpty(_menuName))
            {
                EditorUtility.DisplayDialog("Menu already exists.", "Please specify a valid Menu name to continue.", "Close");
                return;
            }

            var lastFileNumber = Directory.GetFiles(beatsDir, "*.meta", SearchOption.AllDirectories).Length + 1;
            FileUtil.CopyFileOrDirectory($"{scriptsDir}New Menu.unity", $"{beatsDir}{_menuName}.unity");
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();
            Object obj = AssetDatabase.LoadAssetAtPath<Object>($"{beatsDir}{_menuName}.unity");
            Selection.activeObject = obj;

            var original = EditorBuildSettings.scenes;
            var newSettings = new EditorBuildSettingsScene[original.Length + 1];
            System.Array.Copy(original, newSettings, original.Length);
            var sceneToAdd = new EditorBuildSettingsScene($"{beatsDir}{_menuName}.unity", true);
            newSettings[newSettings.Length - 1] = sceneToAdd;
            EditorBuildSettings.scenes = newSettings;
        }
#endif
    }
}
