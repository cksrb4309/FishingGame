using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class CircleDetector : MonoBehaviour
{
    PlayerUIController uiController = null;
    private void Awake()
    {
        uiController = GetComponentInParent<PlayerUIController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
        {
            uiController.AddInteractable(interactable);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
        {
            uiController.RemoveInteractable(interactable);
        }
    }
}
