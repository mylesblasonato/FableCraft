using System;
using UnityEngine;
using TMPro;

namespace FableCraft
{
    [CreateAssetMenu(fileName = "StoryNode", menuName = "AIE/FableCraft/Create Story Node...")]
    public class StoryNode : ScriptableObject, IStoryNode
    {
        [SerializeField] string[] _storyTexts;
        [SerializeField] AudioClip[] _voClips;
        [SerializeField] IStoryOption[] _storyOption;
        [SerializeField] float _voVolume = 1f;

        public string[] StoryTexts => _storyTexts;

        public void Play(int nodeIndex, TextMeshProUGUI storyTextContainer, TextEffect textEffect)
        {
            if (_voClips.Length > 0)
                FableAudioManager.Instance.PlayVo(_voClips[nodeIndex], _voVolume);

            if (textEffect != null)
                textEffect.Play(_storyTexts[nodeIndex], storyTextContainer);
            else
                storyTextContainer.text = _storyTexts[nodeIndex];
        }
    }
}
    
