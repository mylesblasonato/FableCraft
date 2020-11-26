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
<<<<<<< HEAD
            FableManager.Instance.Checkpoint(ConnectedCheckpointName, StoryOptionIndex);
=======
>>>>>>> main
        }

        public void SelectOption()
        {
            FableManager.Instance.HideOptions();
            FableManager.Instance._optionSelected = true;
<<<<<<< HEAD
            FableManager.Instance._triggerDialogue?.Invoke(ConnectedCheckpointName, StoryOptionIndex.ToString());
            FableManager.Instance.Checkpoint(ConnectedCheckpointName, StoryOptionIndex);
=======

            FableManager.Instance._triggerDialogue?.Invoke(ConnectedCheckpointName, StoryOptionIndex.ToString());
            //CustomEvent.Trigger(FableManager.Instance.gameObject, ConnectedCheckpointName, StoryOptionIndex);
>>>>>>> main
        }
    }
}