using Bolt;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FableCraft
{
    public class FableManager : MonoBehaviour, IFableManager
    {
        [SerializeField] FableData _fableData;
        [SerializeField] TextMeshProUGUI _storyTextContainer;
        [SerializeField] GameObject _optionButtonsContainer, _optionButtonPrefab;
        [SerializeField] FableController _continueButton;

        Scene _currentScene;
        TextEffect _textEffect;
        GameObject _textEffectGO = null;
        bool _loadingGame = false;
        public bool _optionSelected = false;
        string _currentCheckpoint = "Checkpoint 1";
        int _selectedOptionPath = 0;

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
            LoadCheckpoint(_fableData);
        }

        public void LoadCheckpoint(FableData data)
        {
            _currentCheckpoint = data.CurrentCheckpointName;
            StopAllCoroutines();
            CustomEvent.Trigger(Instance.gameObject, _currentCheckpoint, _currentCheckpoint);
            _loadingGame = true;          
        }
        
        public void Checkpoint(string checkpointName)
        {
            if (!_loadingGame)
            {
                _fableData.CurrentCheckpointName = checkpointName;
                _fableData.CurrentScene = _currentScene;
                _fableData.SaveFable();
            }

            if (string.Compare(checkpointName, _fableData.CurrentCheckpointName) == 0)
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

        public void Play(StoryNode storyNode, int nodeIndex, int connectedOption, int index)
        {
            if (_loadingGame) return;
            _storyTextContainer.text = "";
            storyNode.Play(
                index,
                _storyTextContainer,
                _textEffect != null ?
                    _textEffectGO.GetComponent<TextEffect>() : null);
        }

        public void AddOption(string name, string connectedOptionName, float width, float height)
        {
            if (_loadingGame)
            {
                CustomEvent.Trigger(FableManager.Instance.gameObject, connectedOptionName, 0);
                return;
            }

            var optionBtn = Instantiate(_optionButtonPrefab);
            optionBtn.transform.SetParent(_optionButtonsContainer.transform);
            optionBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
            optionBtn.transform.localPosition = new Vector3(-width, -height, 0f);
            optionBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
            optionBtn.transform.localScale = new Vector3(1f, 1f, 1f);
            optionBtn.GetComponent<FableController>().ConnectedCheckpointName = connectedOptionName;
        }

        public void HideOptions()
        {
            foreach (Transform go in _optionButtonsContainer.transform)
            {
                Destroy(go.gameObject);
            }
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
