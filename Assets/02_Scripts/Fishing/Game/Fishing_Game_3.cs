public class Fishing_Game_3 : Fishing_Game
{
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        PoolManager.CreatePool<AttackProjectile>(ObjectPoolID.AttackProjectile, 4);
    }
    public override void StartFishing()
    {
        base.StartFishing();
        
        SetCanvas(gameIndex: 3);

        FishController.Instance.Setting(this, (FishingMethodData_Game_1)FishingData.MethodData);

        cursorController.Setting();
    }
    public override void CancelFishing()
    {
        base.CancelFishing();

        FishController.Instance.Cancel();
        cursorController.Cancel();
    }
    public override void Complete()
    {
        base.Complete();
    }
}
