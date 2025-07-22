using Dialogue;
using System.Collections.Generic;
using UnityEngine;

namespace Npc
{
    [System.Serializable]
    public class NpcAffinityTracker
    {
        [SerializeField] List<AffinityThresholdEvent> thresholdEvents;

        public void CheckAffinityEvents(NpcObj npc)
        {
            int affinity = npc.GetAffinity();

            foreach (var evt in thresholdEvents)
            {
                if (!evt.triggered && affinity >= evt.threshold)
                {
                    evt.triggered = true;
                    evt.onThresholdReached?.Invoke();
                }
            }
        }
    }
}
