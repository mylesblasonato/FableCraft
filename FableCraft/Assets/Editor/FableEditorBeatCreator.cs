﻿using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    public class FableEditorBeatCreator : EditorWindow
    {
        string _dirPath = "";
        string _beatName = "";
        Scene _newScene;

#if UNITY_EDITOR
        [MenuItem("FableCraft/Create New FableCraft Beat")]
        public static void CreateNewFable()
        {
            FableEditorBeatCreator window = ScriptableObject.CreateInstance<FableEditorBeatCreator>();
            window.position = new Rect(Screen.width / 2 + 250, Screen.height / 2, 250f, 150f);
            window.titleContent = new GUIContent("Beat Creator");
            window.Show(true);
        }

        [MenuItem("FableCraft/Remove Deleted Scenes")]
        public static void CleanUpDeletedScenes()
        {
            var currentScenes = EditorBuildSettings.scenes;
            var filteredScenes = currentScenes.Where(ebss => File.Exists(ebss.path)).ToArray();
            EditorBuildSettings.scenes = filteredScenes;
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
            _beatName = EditorGUILayout.TextField("Beat Title", _beatName);
            if (GUILayout.Button("Create"))
            {
                OnClickSaveBeat();
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

        void OnClickSaveBeat()
        {
            var scriptsDir = "Assets/Scripts/FableCraft/Scenes/";
            var beatsDir = $"Assets/{_dirPath}/";

            _beatName = _beatName.Trim();

            if (string.IsNullOrEmpty(_beatName))
            {
                EditorUtility.DisplayDialog("Beat already exists.", "Please specify a valid Beat name to continue.", "Close");
                return;
            }

            var lastFileNumber = Directory.GetFiles(beatsDir, "*.meta", SearchOption.AllDirectories).Length + 1;
            FileUtil.CopyFileOrDirectory($"{scriptsDir}New Beat.unity", $"{beatsDir}{_beatName}.unity");
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();
            Object obj = AssetDatabase.LoadAssetAtPath<Object>($"{beatsDir}{_beatName}.unity");
            Selection.activeObject = obj;

            var original = EditorBuildSettings.scenes;
            var newSettings = new EditorBuildSettingsScene[original.Length + 1];
            System.Array.Copy(original, newSettings, original.Length);
            var sceneToAdd = new EditorBuildSettingsScene($"{beatsDir}{_beatName}.unity", true);
            newSettings[newSettings.Length - 1] = sceneToAdd;
            EditorBuildSettings.scenes = newSettings;
        }
#endif
    }
}
