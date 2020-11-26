using Bolt;
using Ludiq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FableCraft
{
    [TypeIcon(typeof(Vector2))] // Choose the type icon. @32x - headers; @16x -ports and fuzzy finder categories. Located in Editor Default Resources.
    [UnitTitle("Add Story Options")] // Sets the actual named title of the unit, this is used in the Fuzzy Finder.
    [UnitCategory("FableCraft")] // Sets unit category in Fuzzy Finder. Subfolders are matched and created.
    public class StoryOption : EventUnit<EmptyEventArgs>, IStoryOption
    {
        private List<KeyValuePair<string, ValueInput>> options;
        private List<KeyValuePair<string, ControlOutput>> branches;
        private Flow _flow;

        [Inspectable, Serialize]
        public float buttonWidth { get; set; } = 150f;

        [Inspectable, Serialize]
        public float buttonHeight { get; set; } = 34f;

        [Inspectable, Serialize]
        public List<string> storyOptions { get; set; } = new List<string>();

        [DoNotSerialize, PortLabelHidden]
        public ControlInput enter { get; private set; }

        [Inspectable, Serialize]
        public string _storyOptionsName;

        [Inspectable, Serialize]
        public List<string> optionBranches { get; set; } = new List<string>();

<<<<<<< HEAD
        Action<string, string> dh;

=======
>>>>>>> main
        protected override bool register => false;

        protected override void Definition()
        {
            enter = ControlInput("enter", StartAction);
            branches = new List<KeyValuePair<string, ControlOutput>>();
            foreach (var optionBranch in optionBranches)
            {
                var key = optionBranch;
                if (!controlOutputs.Contains(key))
                {
                    var branch = ControlOutput(key);
                    branches.Add(new KeyValuePair<string, ControlOutput>(optionBranch, branch));
                }
            }
        }

        public override void StartListening(GraphStack stack)
        {
            base.StartListening(stack);
            var data = stack.GetElementData<Data>(this);
            var reference = stack.ToReference();
            Action<string, string> handler = (_storyOptionsName, eventName) => TriggerOption(reference, _storyOptionsName, eventName);
<<<<<<< HEAD
            data.handler = handler;
            dh = handler;          
=======
            data.handler = handler;         
            FableManager.Instance._triggerDialogue += handler;
>>>>>>> main
        }

        public override void StopListening(GraphStack stack)
        {
            base.StopListening(stack);
            var data = stack.GetElementData<Data>(this);
            var reference = stack.ToReference();
            Action<string, string> handler = (_storyOptionsName, eventName) => TriggerOption(reference, _storyOptionsName, eventName);
            data.handler = handler;
<<<<<<< HEAD
            dh = handler;            
=======
            FableManager.Instance._triggerDialogue -= handler;
>>>>>>> main
        }

        private ControlOutput StartAction(Flow flow)
        {
            _flow = flow;
<<<<<<< HEAD
=======

>>>>>>> main
            for (int i = 0; i < storyOptions.Count; i++)
            {
                FableManager.Instance.AddOption(storyOptions[i], _storyOptionsName, i, buttonWidth, buttonHeight);
            }
<<<<<<< HEAD
            FableManager.Instance._triggerDialogue += dh;
=======
>>>>>>> main
            return null;
        }

        private void TriggerOption(GraphReference reference, string storyOptionsName, string eventName)
        {
<<<<<<< HEAD
            if (!FableManager.Instance._optionSelected) return;
            foreach (var branch in branches)
            {
                if (branch.Key == eventName)
                {
                    using (var flow = Flow.New(reference))
                    {                       
                        FableManager.Instance._optionSelected = false;
                        FableManager.Instance._triggerDialogue -= dh;
                        flow.Invoke(branch.Value);
=======
            foreach (var branch in branches)
            {
                if (_storyOptionsName == FableManager.Instance.CurrentCheckpoint)
                {
                    if (branch.Key == eventName)
                    {
                        using (var flow = Flow.New(reference))
                        {
                            flow.Invoke(branch.Value);
                        }
>>>>>>> main
                    }
                }
            }
        }
    }
}
