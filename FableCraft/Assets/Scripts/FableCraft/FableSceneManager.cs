using System.Collections.Generic;
using UnityEditor;
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
		public int[] _excludedSceneIndexes;

		// Initialize the singleton instance.
		void Awake()
		{
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
				Destroy(gameObject);
			DontDestroyOnLoad(gameObject);
		}

		public void LoadScene(int newSceneBuildIndex, bool isLoadingScene)
		{
			LoadingScene = isLoadingScene;
			if (LoadingScene)
			{
				LoadingScene = false;
				if (USM.SceneManager.GetActiveScene().buildIndex == newSceneBuildIndex) return;
				USM.SceneManager.LoadScene(newSceneBuildIndex, LoadSceneMode.Single);
			}
		}

		public bool IsExcludedScene(int index)
		{
			foreach (var id in _excludedSceneIndexes)
			{
				if (id == index)
					return true;
			}

			return false;
		}
	}
}