using System.Collections;
using System.Collections.Generic;
using Bolt;
using UnityEngine;

namespace FableCraft
{
    public class FableController : MonoBehaviour, IFableController
    {
        public void ContinueStory()
        {
            CustomEvent.Trigger(FableManager.Instance.gameObject, "Continue");
            FableManager.Instance.CurrentStoryNode++;
        }
    }
}