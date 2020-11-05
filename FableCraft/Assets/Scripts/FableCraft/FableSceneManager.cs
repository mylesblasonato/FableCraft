using UnityEngine;
using UnityEngine.SceneManagement;
using USM = UnityEngine.SceneManagement;

namespace FableCraft
{
    public class FableSceneManager : MonoBehaviour, IFableSceneManager
    {
		// Singleton instance.
		public static FableSceneManager Instance = null;

		// Initialize the singleton instance.
		void Awake()
		{
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
				Destroy(gameObject);
			//DontDestroyOnLoad(gameObject);
		}

		public void LoadScene(string scene)
		{
			USM.SceneManager.LoadScene(scene);
		}
    }
}