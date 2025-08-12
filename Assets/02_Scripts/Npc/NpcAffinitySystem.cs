using System.Collections.Generic;
using UnityEngine;

namespace Npc
{
    public static class NpcAffinitySystem
    {
        private static Dictionary<NpcName, int> affinityData = new Dictionary<NpcName, int>();

        public static int GetAffinity(NpcName npcName)
        {
            if (!affinityData.TryGetValue(npcName, out int value)) affinityData[npcName] = 0; // 기본값 0

            return affinityData[npcName];
        }

        public static void AddAffinity(NpcName npcName, int amount)
        {
            int current = GetAffinity(npcName);

            affinityData[npcName] = Mathf.Clamp(current + amount, 0, 100); // 호감도 0~100 제한
        }

        public static void SetAffinity(NpcName npcName, int value)
        {
            affinityData[npcName] = Mathf.Clamp(value, 0, 100);
        }
    }
}
