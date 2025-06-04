using UnityEngine;

public interface IInteractable
{
    public void Interact();
    public void Release();
    public Vector3 GetPosition();
}
