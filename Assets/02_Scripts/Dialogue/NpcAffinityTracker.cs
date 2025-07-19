using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class NpcAffinityTracker
{
    [SerializeField] List<AffinityThresholdEvent> thresholdEvents;

    public void CheckAffinityEvents(Npc npc)
    {
        int affinity = npc.Affinity;

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