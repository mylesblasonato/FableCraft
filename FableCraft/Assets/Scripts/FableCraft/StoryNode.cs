using System;
using UnityEngine;
using TMPro;

namespace FableCraft
{
    [CreateAssetMenu(fileName = "StoryNode", menuName = "AIE/FableCraft/Create Story Node...")]
    public class StoryNode : ScriptableObject, IStoryNode
    {
        [TextArea(50, 100)]
        [SerializeField] string _storyText = "";
        
        [SerializeField] Texture _storyImage;
        [SerializeField] IStoryOption[] _storyOption;

        public string StoryText => _storyText;

        public void Play(TextMeshProUGUI storyTextContainer, int index, TextEffect textEffect, float duration = 0, bool eventTriggered = false)
        {
            if (FableManager.Instance.CurrentStoryNode == index)
            {
                if (textEffect != null)
                    textEffect.Play(_storyText, duration, storyTextContainer);
                else
                    storyTextContainer.text = _storyText;

                if (!eventTriggered)
                    FableManager.Instance.CurrentStoryNode++;
            }
        }
    }
}
    
