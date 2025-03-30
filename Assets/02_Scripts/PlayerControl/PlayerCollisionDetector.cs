using UnityEngine;

public class PlayerCollisionDetector : MonoBehaviour
{
    PlayerMove playerMove;
    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        playerMove.HandleCollision(collision);
    }
}
