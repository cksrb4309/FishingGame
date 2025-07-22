using Dialogue;
using System.Collections.Generic;
using UnityEngine;

namespace Npc
{
    public class NpcObj : MonoBehaviour, IInteractable
    {
        [Header("NPC 데이터")]
        [SerializeField] private NpcData npcData;

        [Header("서브 컴포넌트")]
        [SerializeField] private Quest.QuestGiver questGiver;

        [SerializeField] private ConditionalDialogueManager dialogueManager;

        private bool hasGivenGift = false;

        private void Start()
        {
            InitializeNpc();
        }

        private void InitializeNpc()
        {
            dialogueManager = npcData.dialogueManager;
        }

        public Vector3 GetPosition() => transform.position;
        public NpcName GetNpcName() => npcData.npcNameType;

        public virtual void Interact()
        {
            DialogueTree tree = dialogueManager.GetAppropriateDialogue(this);

            if (tree != null)
            {
                DialogueUIController.Instance.StartDialogue(tree);
            }
            else
            {
                Quest.QuestUIController.Instance.ShowQuestUI();
            }
        }

        public virtual void Release() { }

        public void IncreaseAffinity(int amount)
        {
            NpcAffinitySystem.AddAffinity(npcData.npcNameType, amount);

            Debug.Log($"{npcData.name} 호감도: {NpcAffinitySystem.GetAffinity(npcData.npcNameType)}");
        }

        public int GetAffinity()
        {
            return NpcAffinitySystem.GetAffinity(npcData.npcNameType);
        }

        public bool HasGivenGift => hasGivenGift;

        public void GiveGift()
        {
            if (hasGivenGift) return;

            hasGivenGift = true;

            IncreaseAffinity(10);
        }
    }
}
