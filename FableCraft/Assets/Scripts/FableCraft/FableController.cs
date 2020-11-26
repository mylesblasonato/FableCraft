using System.Collections;
using System.Collections.Generic;
using Bolt;
using UnityEngine;

namespace FableCraft
{
    public class FableController : MonoBehaviour, IFableController
    {
        public string ConnectedCheckpointName { get; set; }
        public int StoryOptionIndex { get; set; }

        public void ContinueStory()
        {
            CustomEvent.Trigger(FableManager.Instance.gameObject, "Continue");
            FableManager.Instance.Checkpoint(ConnectedCheckpointName, StoryOptionIndex);
        }

        public void SelectOption()
        {
            FableManager.Instance.HideOptions();
            FableManager.Instance._optionSelected = true;
            FableManager.Instance._triggerDialogue?.Invoke(ConnectedCheckpointName, StoryOptionIndex.ToString());
            FableManager.Instance.Checkpoint(ConnectedCheckpointName, StoryOptionIndex);
        }
    }
}