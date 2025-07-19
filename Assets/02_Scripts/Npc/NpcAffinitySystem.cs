using System.Collections.Generic;
using UnityEngine;

public static class NpcAffinitySystem
{
    private static Dictionary<int, int> affinityData = new Dictionary<int, int>();

    public static int GetAffinity(int npcId)
    {
        if (!affinityData.TryGetValue(npcId, out int value)) affinityData[npcId] = 0; // 기본값 0

        return affinityData[npcId];
    }

    public static void AddAffinity(int npcId, int amount)
    {
        int current = GetAffinity(npcId);

        affinityData[npcId] = Mathf.Clamp(current + amount, 0, 100); // 호감도 0~100 제한
    }

    public static void SetAffinity(int npcId, int value)
    {
        affinityData[npcId] = Mathf.Clamp(value, 0, 100);
    }
}
