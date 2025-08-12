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
        
        private InteractGuideImage guideImage;

        [Header("참조")]
        [SerializeField] private Sprite interactGuideImage;

        public bool HasGivenGift => hasGivenGift;
        private bool hasGivenGift = false;

        private void Start()
        {
            InitializeNpc();
        }

        private void InitializeNpc()
        {
            dialogueManager = npcData.dialogueManager;
            guideImage = GetComponent<InteractGuideImage>();
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
        public void IncreaseAffinity(int amount)
        {
            NpcAffinitySystem.AddAffinity(npcData.npcNameType, amount);

            Debug.Log($"{npcData.name} 호감도: {NpcAffinitySystem.GetAffinity(npcData.npcNameType)}");
        }
        public int GetAffinity()
        {
            return NpcAffinitySystem.GetAffinity(npcData.npcNameType);
        }
        public void GiveGift()
        {
            if (hasGivenGift) return;

            hasGivenGift = true;

            IncreaseAffinity(10);
        }
        public void Select()
        {
            guideImage.Show(interactGuideImage);
        }
        public virtual void Release()
        {
            guideImage.Hide(interactGuideImage);
        }
    }
}
