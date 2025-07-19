using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour, IInteractable
{

    [SerializeField] QuestGiver giver = null;

    [SerializeField] List<ConditionalDialogueTree> dialogueOptions;
    public Vector3 GetPosition() => transform.position;
    public virtual void Interact()
    {
        if (giver != null) giver.Interact();
    }
    //public void Interact()
    //{
    //    DialogueManager.Instance.StartDialogue(GetDialogueTree());
    //}
    public virtual void Release()
    {

    }
    public DialogueTree GetDialogueTree()
    {
        foreach (var option in dialogueOptions)
        {
            if (option.CheckCondition(this)) return option.dialogueTree;
        }

        Debug.LogWarning("조건에 맞는 트리 없음 -> 하나는 맞아야함 -> 고쳐 이자식아");

        return null;
    }



    [SerializeField] private int affinity;
    public int Affinity => affinity;
    NpcAffinityTracker tracker = null;
    public void AddAffinity(int amount)
    {
        if (tracker == null) tracker = GetComponent<NpcAffinityTracker>();

        affinity += amount;
        tracker.CheckAffinityEvents(this);
    }
}
