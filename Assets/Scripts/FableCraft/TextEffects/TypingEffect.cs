using System.Collections;
using TMPro;
using UnityEngine;

namespace FableCraft.TextEffects
{
    public class TypingEffect : TextEffect
    {
        TextMeshProUGUI _textContainer;
        float _typeDuration;
        char[] _chars;

        public override void Play(string text, TextMeshProUGUI textContainer)
        {
            _textContainer = textContainer;
            _textContainer.text = "";
            _chars = text.ToCharArray();
            _typeDuration = _duration / _chars.Length;

            if(_clip != null)
                FableAudioManager.Instance.PlayTextEffectAudio(_clip, _volume);

            StopAllCoroutines();
            StartCoroutine(Effect());
        }

        IEnumerator Effect()
        {
            foreach (var letter in (_chars))
            {
                _textContainer.text += letter;
                yield return new WaitForSeconds(_typeDuration);
            }

            FableAudioManager.Instance.StopTextEffectAudio();
        }
    }
}
