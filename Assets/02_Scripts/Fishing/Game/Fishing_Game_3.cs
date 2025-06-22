public class Fishing_Game_3 : Fishing_Game
{
    CursorController_Game_3 cursorController;
    protected override void Awake()
    {
        cursorController = GetComponentInChildren<CursorController_Game_3>(true);
    }
    private void Start()
    {
        PoolManager.CreatePool<AttackProjectile>(ObjectPoolID.AttackProjectile, 4);
    }
    public override void StartFishing()
    {
        base.StartFishing();
        
        game123.alpha = 1;
        game4.alpha = 0;

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
