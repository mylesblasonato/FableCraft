using UnityEngine;
using UnityEngine.SceneManagement;
using USM = UnityEngine.SceneManagement;

namespace FableCraft
{
	public class FableSceneManager : MonoBehaviour, IFableSceneManager
	{
		// Singleton instance.
		public static FableSceneManager Instance = null;
		public bool LoadingScene { get; private set; }

		// Initialize the singleton instance.
		void Awake()
		{
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
				Destroy(gameObject);
		}

		public void LoadScene(string newScene, bool isLoadingScene)
		{
			LoadingScene = isLoadingScene;
			if (LoadingScene)
			{
				LoadingScene = false;
				if (USM.SceneManager.GetActiveScene().name == newScene) return;
				USM.SceneManager.LoadScene(newScene, LoadSceneMode.Single);
			}
		}
    }
}