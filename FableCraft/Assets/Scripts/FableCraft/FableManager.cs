using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FableCraft
{
    public class FableManager : MonoBehaviour, IFableManager
    {
        [SerializeField] TextMeshProUGUI _storyTextContainer;
        [SerializeField] FableController _continueButton;

        GameObject _textEffect = null;
        float _textEffectDuration = 0f;
        
        public int CurrentStoryNode { get; set; } = 0;
        public static FableManager Instance { get; private set; }

        // Start is called before the first frame update
        void Awake() => Instance = this;

        void Start()
        {
            _continueButton.gameObject.SetActive(false);
        }

        public void Play(StoryNode storyNode, int index, bool eventTriggered = false)
        {
            if (_textEffect != null)
            {
                var textEffectGO = Instantiate(_textEffect);
                storyNode.Play(_storyTextContainer, index, textEffectGO.GetComponent<TextEffect>(), _textEffectDuration,
                    eventTriggered);
            }
            else
            {
                storyNode.Play(_storyTextContainer, index, null, _textEffectDuration,
                    eventTriggered);
            }
        }

        public void ChangeTextEffect(GameObject textEffect, float duration)
        {
            if (textEffect != null)
            {
                _textEffect = textEffect;
                _textEffectDuration = duration;
            }
        }

        public void ShowContinueButton(bool show)
        {
            _continueButton.gameObject.SetActive(show);
        }

        public void ChangeImage(Texture texture, Color color, string imageGameObjectToChange)
        {
            var container = GameObject.Find(imageGameObjectToChange);
            container.GetComponent<RawImage>().color = color;
            container.GetComponent<RawImage>().texture = texture;
        }
    }
}
