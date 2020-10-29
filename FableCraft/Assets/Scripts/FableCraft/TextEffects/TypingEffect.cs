using System.Collections;
using TMPro;
using UnityEngine;

namespace FableCraft.TextEffects
{
    public class TypingEffect : TextEffect
    {
        TextMeshProUGUI _textContainer;
        float _duration;
        char[] _chars;
 
        public override void Play(string text, float duration, TextMeshProUGUI textContainer)
        {
            _duration = duration;
            _textContainer = textContainer;
            _chars = text.ToCharArray();
            
            StopAllCoroutines();
            StartCoroutine(Effect());
        }

        IEnumerator Effect()
        {
            foreach (var letter in (_chars))
            {
                _textContainer.text += letter;
                yield return new WaitForSeconds(_duration);
            }
        }
    }
}
