using DG.Tweening;
using UnityEngine;
using VContainer;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float combatIdleDuration = 2f;

    [Inject] PlayerEffect playerEffect;

    Tween parryResetTween = null;
    int parryIndex = 1;
    int ParryIndex
    {
        get
        {
            int ret = parryIndex++;

            if (parryIndex == 4)
                parryIndex = 1;

            if (parryResetTween == null)
                parryResetTween = DOVirtual.DelayedCall(combatIdleDuration, ParryReset).SetAutoKill(false);
            
            else
                parryResetTween.Restart();
            
            return ret;
        }
    }
    public void Parry()
    {
        int parryIndex = ParryIndex;
        animator.SetTrigger("Parry_" + parryIndex.ToString());
        playerEffect.Parry(parryIndex);
    }
    void ParryReset()
    {
        parryIndex = 1;

        animator.SetTrigger("Idle");
    }
    private void Awake()
    {
        if (animator == null)
            animator = GetComponentInParent<Animator>();
    }
}
