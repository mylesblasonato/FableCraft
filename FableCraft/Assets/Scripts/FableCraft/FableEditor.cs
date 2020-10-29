using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using FableCraft;

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
            //var files = Directory.GetFiles($"{beatsDir}", "*.meta", SearchOption.AllDirectories);
            //var fileNumbers = files.Where(z => z.Contains("New Beat")).Select(z =>
            //{
            //    try
            //    {
            //        return int.Parse(z.Split(' ')[2]);
            //    }
            //    catch
            //    {
            //        return 0;
            //    }
            //}).ToList();
            //fileNumbers.Sort();
            //var lastFileNumber = fileNumbers.LastOrDefault();
            //var numFiles = files.Length;

            FableEditor window = ScriptableObject.CreateInstance<FableEditor>();
            window.position = new Rect(Screen.width / 2 + 250, Screen.height / 2, _beatCreatorWindowWidth, _beatCreatorWindowHeight);
            window.titleContent = new GUIContent(_beatCreatorName);
            window.Show(true);
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
        }

#endif
    }
}
