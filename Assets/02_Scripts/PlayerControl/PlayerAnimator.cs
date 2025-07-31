using UnityEngine;

public class PlayerAnimator : MonoBehaviour, IDirectionable
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    
    public void UpdateDirection(PlayerDir dir)
    {
        switch (dir)
        {
            case PlayerDir.TopLeft:
                animator.SetBool("IsFront", false);
                spriteRenderer.flipX = true; break;

            case PlayerDir.TopRight:
                animator.SetBool("IsFront", false);
                spriteRenderer.flipX = false; break;

            case PlayerDir.BottomLeft:
                animator.SetBool("IsFront", true);
                spriteRenderer.flipX = true; break;

            case PlayerDir.BottomRight:
                animator.SetBool("IsFront", true);
                spriteRenderer.flipX = false; break;
        }
    }
    public void SetMoveSpeed(float speed)
    {
        animator.SetFloat("Move", speed);
    }
    private void OnEnable()
    {
        DirectionManager.Instance.RegisterDirectionable(this);
    }
    private void OnDisable()
    {
        DirectionManager.Instance.UnregisterDirectionable(this);
    }
}
