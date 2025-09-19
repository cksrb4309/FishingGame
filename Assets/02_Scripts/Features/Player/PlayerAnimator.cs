using DG.Tweening;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;
    private void Awake()
    {
        if (animator == null)
            animator = GetComponentInParent<Animator>();
    }

    Tween parryResetTween = null;
    int parryIndex = 0;
    int ParryIndex
    {
        get
        {
            int ret = parryIndex;

            if (++parryIndex > 2) parryIndex = 0;

            if (parryResetTween == null)
                parryResetTween = DOVirtual.DelayedCall(1f, ParryReset).SetAutoKill(false);
            
            else
                parryResetTween.Restart();
            
            return ret;
        }
    }
    public void Parry()
    {
        animator.SetTrigger("Parry" + ParryIndex.ToString());
    }
    void ParryReset()
    {
        parryIndex = 0;
    }
}
