using System;

namespace Dialogue
{
    [Serializable]
    public class AffinityThresholdEvent
    {
        public int threshold;               // 25, 50, 75, 100 등
        [NonSerialized] public bool triggered;              // 중복 발생 방지
        public UnityEngine.Events.UnityEvent onThresholdReached;
    }
}