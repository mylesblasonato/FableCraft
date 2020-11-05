using Bolt;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace FableCraft
{
    public class FableManager : MonoBehaviour, IFableManager
    {
        [SerializeField] TextMeshProUGUI _storyTextContainer;
        [SerializeField] FableController _continueButton;

        TextEffect _textEffect = null;
        GameObject _textEffectGO = null;
        float _textEffectDuration = 0f;

        public int CurrentStoryNode { get; set; } = 0;
        public static FableManager Instance { get; private set; }

        // Start is called before the first frame update
        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
            //DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            _continueButton.gameObject.SetActive(false);
        }

        public void LoadGame(int index)
        {
            StopAllCoroutines();
            CurrentStoryNode = index;
            CustomEvent.Trigger(gameObject, "Checkpoint", index);
        }

        public void LoadBeat(SceneAsset scene)
        {
            if (scene != null)
                FableSceneManager.Instance.LoadScene(scene.name);
        }

        public void Play(StoryNode storyNode, int index, bool eventTriggered = false)
        {
            if (CurrentStoryNode != index) return;

            StopAllCoroutines();
            _storyTextContainer.text = "";

            if (_textEffect != null)
            {
                storyNode.Play(_storyTextContainer, index, _textEffectGO.GetComponent<TextEffect>(), _textEffectDuration,
                    eventTriggered);
            }
            else
            {
                storyNode.Play(_storyTextContainer, index, null, _textEffectDuration,
                    eventTriggered);
            }
        }

        public void PlayMusic(AudioClip clip, float volume = 1)
        {
            FableAudioManager.Instance.PlayMusic(clip, volume);
        }

        public void PlaySFX(AudioClip clip, float volume = 1)
        {
            FableAudioManager.Instance.PlaySFX(clip, volume);
        }

        public void StopAudio()
        {
            FableAudioManager.Instance.StopAudio();
        }

        public void ChangeTextEffect(GameObject textEffect, float duration, AudioClip clip, float volume)
        {
            if (textEffect != null)
            {
                Destroy(_textEffectGO);
                _textEffect = textEffect.GetComponent<TextEffect>();
                _textEffectGO = Instantiate(textEffect);
                _textEffectDuration = duration;

                if (clip != null)
                    _textEffectGO.GetComponent<TextEffect>().SetProperties(clip, volume);
            }
        }

        public void ChangeImage(Texture texture, Color color, string imageGameObjectToChange)
        {
            var container = GameObject.Find(imageGameObjectToChange);
            container.GetComponent<RawImage>().color = color;
            container.GetComponent<RawImage>().texture = texture;
        }

        public void ShowContinueButton(bool show)
        {
            _continueButton.gameObject.SetActive(show);
        }
    }
}
