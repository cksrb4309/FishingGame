using UnityEngine;
using System.Collections.Generic;
using VInspector;

namespace Npc
{
    public class NpcDatabase : MonoBehaviour
    {
        public static NpcDatabase Instance { get; private set; }

        [SerializeField] SerializedDictionary<NpcName, NpcData> npcDataDict;

        private void Awake()
        {
            Instance = this;
        }

        public NpcData GetNpcData(NpcName npcName)
        {
            npcDataDict.TryGetValue(npcName, out var data);

            return data;
        }
    }
}
