using VContainer;
using VContainer.Unity;
using UnityEngine;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] PlayerAnimator playerAnimator;
    [SerializeField] PlayerEffect playerEffect;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent<PlayerAnimator>(playerAnimator);
        builder.RegisterComponent<PlayerEffect>(playerEffect);
    }
}
