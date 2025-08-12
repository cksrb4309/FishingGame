using UnityEngine;

public interface IInteractable
{
    public void Interact();
    public void Select();
    public void Release();
    public Vector3 GetPosition();
}
