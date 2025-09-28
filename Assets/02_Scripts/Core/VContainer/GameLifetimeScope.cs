using VContainer;
using VContainer.Unity;
using UnityEngine;

public class GameLifetimeScope : LifetimeScope
{
    private static GameLifetimeScope instance = null;

    [SerializeField] PlayerAnimator playerAnimator;
    [SerializeField] PlayerEffect playerEffect;

    public static IObjectResolver ObjectResolver => instance.Container;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent<PlayerAnimator>(playerAnimator);
        builder.RegisterComponent<PlayerEffect>(playerEffect);
    }
    protected override void Awake()
    {
        base.Awake();

        instance = this;
    } 
}
