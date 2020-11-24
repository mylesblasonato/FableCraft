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
        }

        public void SelectOption()
        {
            FableManager.Instance.HideOptions();
            FableManager.Instance._optionSelected = true;
            CustomEvent.Trigger(FableManager.Instance.gameObject, ConnectedCheckpointName, StoryOptionIndex);
        }
    }
}