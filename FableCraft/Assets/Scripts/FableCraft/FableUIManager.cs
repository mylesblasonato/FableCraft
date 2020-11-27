using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FableCraft
{
    public class FableUIManager : MonoBehaviour, IFableUIManager
    {
        public GameObject[] _imageContainers;
        public GameObject[] _textContainers;

		public static FableUIManager Instance { get; private set; }

		// Initialize the singleton instance.
		void Awake()
		{
			if (Instance == null)
				Instance = this;
			else if (Instance != this)
				Destroy(gameObject);
			//DontDestroyOnLoad(gameObject);
		}

		public GameObject GetImageContainer(int imageContainerIndex)
		{
			return _imageContainers[imageContainerIndex];
		}

		public GameObject GetTextContainer(int textContainerIndex)
		{
			return _textContainers[textContainerIndex];
		}
    }
}