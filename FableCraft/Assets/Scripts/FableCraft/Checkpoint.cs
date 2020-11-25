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
        [DoNotSerialize, PortLabelHidden]
        public ControlInput enter { get; private set; }

        [DoNotSerialize, PortLabelHidden]
        public ControlOutput exit { get; private set; }

        public ValueInput checkpointName;

        private List<KeyValuePair<string, ControlOutput>> branches;

        protected override bool register => false;

        protected override void Definition()
        {
            enter = ControlInput("enter", StartAction);
            exit = ControlOutput("exit");
            checkpointName = ValueInput<string>("checkpointName");
        }

        public override void StartListening(GraphStack stack)
        {
            var reference = stack.ToReference();
            var flow = Flow.New(reference);            
            base.StartListening(stack);
            var data = stack.GetElementData<Data>(this);
            Action<string> handler = (eventName) => TriggerOption(reference, eventName);
            data.handler = handler;

            if (flow.GetValue<string>(checkpointName) != FableManager.Instance.CurrentCheckpoint) return;
            FableManager.Instance._checkpoint += handler;
        }

        public override void StopListening(GraphStack stack)
        {
            var reference = stack.ToReference();
            var flow = Flow.New(reference);          
            base.StopListening(stack);
            var data = stack.GetElementData<Data>(this);          
            Action<string> handler = (eventName) => TriggerOption(reference, eventName);
            data.handler = handler;

            if (flow.GetValue<string>(checkpointName) != FableManager.Instance.CurrentCheckpoint) return;
            FableManager.Instance._checkpoint -= handler;
        }

        private ControlOutput StartAction(Flow flow)
        {     
            return null;
        }

        private void TriggerOption(GraphReference reference, string eventName)
        {
            var flow = Flow.New(reference);
           // if (flow.GetValue<string>(name) != FableManager.Instance.CurrentCheckpoint) return;          
            flow?.Invoke(exit);
            //FableManager.Instance.Checkpoint(flow.GetValue<string>(name));
        }
    }
}
