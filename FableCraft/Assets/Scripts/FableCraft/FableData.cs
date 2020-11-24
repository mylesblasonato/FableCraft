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

        [SerializeField] SceneAsset _currentSceneAsset = null;
        [SerializeField] string _currentCheckpointName;
        [SerializeField] int _currentPlayNode= 0;

        public Scene CurrentScene { get => _currentScene; set => _currentScene = value; }
        public SceneAsset CurrentSceneAsset { get => _currentSceneAsset; private set => _currentSceneAsset = value; }
        public string CurrentCheckpointName { get => _currentCheckpointName; set => _currentCheckpointName = value; }
        public int CurrentPlayNode { get => _currentPlayNode; set => _currentPlayNode = value; }

        void OnEnable()
        {
            _savePath = Application.persistentDataPath + "/savegame.fc";
        }

        public void Initialize()
        {
            SceneManager.LoadScene(_currentSceneAsset.name);
            _currentScene = SceneManager.GetActiveScene();
        }

        public void SetSceneAsset(SceneAsset sceneAsset)
        {
            if (sceneAsset.name != "Main Menu")
            {
                _currentSceneAsset = sceneAsset;
            }

            FableSceneManager.Instance.LoadScene(sceneAsset.name, true);
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
