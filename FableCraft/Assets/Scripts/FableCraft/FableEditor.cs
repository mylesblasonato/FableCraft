using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

namespace FableCraft
{
    public class FableEditor : EditorWindow
    {
        [SerializeField] static string _beatCreatorName = "Beat Creator";
        [SerializeField] static float _beatCreatorWindowWidth = 250f;
        [SerializeField] static float _beatCreatorWindowHeight = 150f;
        [SerializeField] static string _beatNameFieldLabel = "Beat Title";

        string beatName = "Please enter a name...";
        Scene _newScene;

#if UNITY_EDITOR
        [MenuItem("FableCraft/Create New Beat")]
        public static void CreateNewFable()
        {
            FableEditor window = ScriptableObject.CreateInstance<FableEditor>();
            window.position = new Rect(Screen.width / 2 + 250, Screen.height / 2, _beatCreatorWindowWidth, _beatCreatorWindowHeight);
            window.titleContent = new GUIContent(_beatCreatorName);
            window.Show(true);
        }

        [MenuItem("FableCraft/Remove Deleted Beats")]
        public static void CleanUpDeletedScenes()
        {
            var currentScenes = EditorBuildSettings.scenes;
            var filteredScenes = currentScenes.Where(ebss => File.Exists(ebss.path)).ToArray();
            EditorBuildSettings.scenes = filteredScenes;
        }

        void OnGUI()
        {
            beatName = EditorGUILayout.TextField(_beatNameFieldLabel, beatName);
            if (GUILayout.Button("Create"))
            {
                OnClickSaveBeat();
                GUIUtility.ExitGUI();
            }
        }

        void OnClickSaveBeat()
        {
            var scriptsDir = "Assets/Scripts/FableCraft/";
            var beatsDir = "Assets/Beats/";

            beatName = beatName.Trim();

            if (string.IsNullOrEmpty(beatName))
            {
                EditorUtility.DisplayDialog("Beat already exists.", "Please specify a valid Beat name to continue.", "Close");
                return;
            }

            var lastFileNumber = Directory.GetFiles(beatsDir, "*.meta", SearchOption.AllDirectories).Length + 1;
            FileUtil.CopyFileOrDirectory($"{scriptsDir}New Beat.unity", $"{beatsDir}{beatName}.unity");
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();
            Object obj = AssetDatabase.LoadAssetAtPath<Object>($"{beatsDir}{beatName}.unity");
            Selection.activeObject = obj;

            var original = EditorBuildSettings.scenes;
            var newSettings = new EditorBuildSettingsScene[original.Length + 1];
            System.Array.Copy(original, newSettings, original.Length);
            var sceneToAdd = new EditorBuildSettingsScene($"{beatsDir}{beatName}.unity", true);
            newSettings[newSettings.Length - 1] = sceneToAdd;
            EditorBuildSettings.scenes = newSettings;
        }

#endif
    }
}
