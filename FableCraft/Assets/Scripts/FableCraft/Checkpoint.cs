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
    [UnitTitle("Add Checkpoint")] // Sets the actual named title of the unit, this is used in the Fuzzy Finder.
    [UnitCategory("FableCraft")] // Sets unit category in Fuzzy Finder. Subfolders are matched and created.
    public class Checkpoint : EventUnit<EmptyEventArgs>, ICheckpoint
    {
        [DoNotSerialize, PortLabelHidden] public ControlInput enter { get; private set; }
        [DoNotSerialize, PortLabelHidden] public ControlOutput exit { get; private set; }
        [Inspectable, Serialize] public List<string> optionBranches { get; set; } = new List<string>();
        
        List<KeyValuePair<string, ControlOutput>> branches;

        public ValueInput checkpointName;
        public Action<string> dh;

        protected override bool register => false;

        protected override void Definition()
        {
            enter = ControlInput("enter", StartAction);
            exit = ControlOutput("exit");
            checkpointName = ValueInput<string>("checkpointName");

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
            Action<string> handler = (eventName) => TriggerOption(reference, eventName);
            data.handler = handler;
            dh = handler;
        }

        public override void StopListening(GraphStack stack)
        {
            base.StopListening(stack);
            var data = stack.GetElementData<Data>(this);
            var reference = stack.ToReference();
            Action<string> handler = (eventName) => TriggerOption(reference, eventName);
            data.handler = handler;
            dh = handler;          
        }

        private ControlOutput StartAction(Flow flow)
        {
            FableManager.Instance._checkpoint += dh;
            if (FableManager.Instance._loadingGame)            
                FableManager.Instance.LoadCheckpoint();
            FableManager.Instance.Checkpoint(flow.GetValue<string>(checkpointName));
            FableManager.Instance._checkpoint?.Invoke(flow.GetValue<string>(checkpointName));
            return null;
        }

        private void TriggerOption(GraphReference reference, string eventName)
        {
            var flow = Flow.New(reference);
            if (flow.GetValue<string>(checkpointName) == eventName)
            {
                using (flow)
                {
                    FableManager.Instance._loadingGame = false;                   
                    FableManager.Instance._checkpoint -= dh;
                    flow.Invoke(exit);
                }
            }
        }
    }
}
