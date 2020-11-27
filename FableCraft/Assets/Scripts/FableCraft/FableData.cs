using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FableCraft
{
    [CreateAssetMenu(fileName = "New Fable Data", menuName = "AIE/FableCraft/Create Save Data...")]
    public class FableData : ScriptableObject, IFableData
    {
        string _savePath = "";
        Scene _currentScene;

        [SerializeField] int _currentSceneAsset = 0;
        [SerializeField] string _currentCheckpointName;
        public Action<string> _checkpointEvent;
        [SerializeField] int _currentPlayNode= 0;
        [SerializeField] FableAttribute[] _attributes;

        public Scene CurrentScene { get => _currentScene; set => _currentScene = value; }
        public int CurrentSceneAsset { get => _currentSceneAsset; private set => _currentSceneAsset = value; }
        public string CurrentCheckpointName { get => _currentCheckpointName; set => _currentCheckpointName = value; }
        public int CurrentPlayNode { get => _currentPlayNode; set => _currentPlayNode = value; }
        public FableAttribute[] Attributes => _attributes;

        void OnEnable()
        {
            _savePath = Application.persistentDataPath + "/savegame.fc";
            Debug.Log(Application.persistentDataPath);
        }

        public void Initialize(int sceneAssetIndex)
        {
            if (!FableSceneManager.Instance.IsExcludedScene(sceneAssetIndex))
                _currentSceneAsset = sceneAssetIndex;
            
            SceneManager.LoadScene(sceneAssetIndex);
            _currentScene = SceneManager.GetActiveScene();
        }

        public void SetSceneAsset(int sceneAssetIndex)
        {
            if (!FableSceneManager.Instance.IsExcludedScene(sceneAssetIndex))
                _currentSceneAsset = sceneAssetIndex;
            
            FableSceneManager.Instance.LoadScene(sceneAssetIndex, true);
            _currentScene = SceneManager.GetActiveScene();
        }

        public void SaveFable()
        {
            var json = JsonUtility.ToJson(this);
            System.IO.File.WriteAllText(_savePath, json);
        }
        
        public void LoadFable()
        {
            if (_savePath == "") return;
            var json = System.IO.File.ReadAllText(_savePath);
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }
}
