using UnityEngine;

public class Npc : MonoBehaviour, IInteractable
{
    [SerializeField] QuestGiver giver = null;
    public Vector3 GetPosition() => transform.position;
    public void Interact()
    {
        if (giver != null) giver.Interact();
    }
    public void Release()
    {

    }
}
