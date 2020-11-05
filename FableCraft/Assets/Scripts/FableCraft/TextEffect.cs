using TMPro;
using UnityEngine;

namespace FableCraft
{
    public class TextEffect : MonoBehaviour
    {
        protected AudioClip _clip;
        protected float _volume;

        public void SetProperties(AudioClip clip, float volume)
        {
            _clip = clip;
            _volume = volume;
        }

        public virtual void Play(string text, float duration, TextMeshProUGUI textContainer)
        {
            textContainer.text = text;
        }
    }
}