using TMPro;
using UnityEngine;

namespace FableCraft
{
    public class TextEffect : MonoBehaviour
    {
        [SerializeField] protected AudioClip _clip;
        [SerializeField] protected float _volume;
        [SerializeField] protected float _duration;

        public void SetProperties(AudioClip clip, float volume, float duration)
        {
            _clip = clip;
            _volume = volume;
            _duration = duration;
        }

        public virtual void Play(string text, TextMeshProUGUI textContainer)
        {
            textContainer.text = text;
        }
    }
}