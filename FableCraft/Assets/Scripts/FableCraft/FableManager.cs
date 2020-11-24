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
            CustomEvent.Trigger(Instance.gameObject, _currentCheckpoint, data.CurrentPlayNode);
            LoadBeat(data.CurrentSceneAsset);
        }

        public void Checkpoint(string checkpointName)
        {
            _fableData.CurrentCheckpointName = checkpointName;
            _fableData.CurrentScene = _currentScene;
            _fableData.SaveFable();
        }

        public void LoadBeat(SceneAsset scene)
        {
            if (scene != null)
                _fableData.SetSceneAsset(scene);
        }

        public void Play(StoryNode storyNode, int index)
        {
            _storyTextContainer.text = "";
            storyNode.Play(
                index,
                _storyTextContainer,
                _textEffect != null ?
                    _textEffectGO.GetComponent<TextEffect>() : null);
        }

        public void AddOption(string name, string connectedOptionName, int optionIndex, float width, float height)
        {
            var optionBtn = Instantiate(_optionButtonPrefab);
            optionBtn.transform.SetParent(_optionButtonsContainer.transform);
            optionBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
            optionBtn.transform.localPosition = new Vector3(-width, -height, 0f);
            optionBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
            optionBtn.transform.localScale = new Vector3(1f, 1f, 1f);
            optionBtn.GetComponent<FableController>().ConnectedCheckpointName = connectedOptionName;
            optionBtn.GetComponent<FableController>().StoryOptionIndex = optionIndex;
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
            FableAudioManager.Instance.PlayMusic(clip, volume);
        }

        public void PlaySfx(int clip, float volume = 1)
        {
            FableAudioManager.Instance.PlaySfx(clip, volume);
        }

        public void StopAudio()
        {
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
            var container = FableUIManager.Instance.GetImageContainer(imageContainerIndex);
            container.GetComponent<Image>().color = color;
            if(sprite)
                container.GetComponent<Image>().sprite = sprite;
            if(shader)
                container.GetComponent<Image>().material = shader;
        }

        public void ShowContinueButton(bool show)
        {
            _continueButton.gameObject.SetActive(show);
        }
    }
}
