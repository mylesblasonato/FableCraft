using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace FableCraft
{
    public class FableManager : MonoBehaviour, IFableManager
    {
        [SerializeField] FableData _fableData;
        [SerializeField] TextMeshProUGUI _storyTextContainer;
        [SerializeField] FableController _continueButton;

        Scene _currentScene;
        TextEffect _textEffect;
        GameObject _textEffectGO = null;
        bool _loadingGame = false;
        int _currentStoryNode = 0;

        public static FableManager Instance { get; private set; }

        // Start is called before the first frame update
        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
            
            #if UNITY_EDITOR
                        
            #else
                _fableData.LoadFable();
            #endif
        }

        void Start()
        {
            _continueButton.gameObject.SetActive(false);           
            LoadCheckpoint(_fableData.CurrentCheckpoint);
        }

        void LoadCheckpoint(int index)
        {
            StopAllCoroutines();

            _currentStoryNode = index;

            if (_currentStoryNode > 0)
                _loadingGame = true;
        }
        
        public void Checkpoint(int index)
        {
            if (!_loadingGame)
            {
                _fableData.CurrentCheckpoint = index;
                _fableData.CurrentScene = _currentScene;
                _fableData.SaveFable();
            }

            if (_currentStoryNode == index)
                _loadingGame = false;
        }

        public void LoadBeat(SceneAsset scene)
        {
            if (_loadingGame) return;
            if (scene != null)
            {
                _fableData.SetSceneAsset(scene);            
            }
        }

        public void Play(StoryNode storyNode, int index)
        {
            if (_loadingGame) return;
            _currentStoryNode++;
            StopAllCoroutines();
            _storyTextContainer.text = "";
            storyNode.Play(
                index,
                _storyTextContainer,
                _textEffect != null ?
                    _textEffectGO.GetComponent<TextEffect>() : null);
        }

        public void PlayMusic(int clip, float volume = 1)
        {
            if (_loadingGame) return;
            FableAudioManager.Instance.PlayMusic(clip, volume);
        }

        public void PlaySfx(int clip, float volume = 1)
        {
            if (_loadingGame) return;
            FableAudioManager.Instance.PlaySfx(clip, volume);
        }

        public void StopAudio()
        {
            if (_loadingGame) return;
            FableAudioManager.Instance.StopAudio();
        }
        
        public void ChangeTextEffect(GameObject textEffect)
        {
            if (textEffect == null) return;
            Destroy(_textEffectGO);
            _textEffect = textEffect.GetComponent<TextEffect>();
            _textEffectGO = Instantiate(textEffect);
        }

        public void ChangeSprite(Sprite sprite, Color color, Material shader, int imageContainerIndex)
        {
            if (_loadingGame) return;
            var container = FableUIManager.Instance.GetImageContainer(imageContainerIndex);
            container.GetComponent<Image>().color = color;
            if(sprite)
                container.GetComponent<Image>().sprite = sprite;
            if(shader)
                container.GetComponent<Image>().material = shader;
        }

        public void ShowContinueButton(bool show)
        {
            if (_loadingGame) return;
            _continueButton.gameObject.SetActive(show);
        }
    }
}
