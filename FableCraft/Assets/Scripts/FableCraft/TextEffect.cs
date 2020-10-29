using TMPro;
using UnityEngine;

namespace FableCraft
{
    public class TextEffect : MonoBehaviour
    {
        public virtual void Play(string text, float duration, TextMeshProUGUI textContainer)
        {
            textContainer.text = text;
        }
    }
}