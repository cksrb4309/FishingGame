public class Fishing_Game_2 : Fishing_Game
{
    CursorController_Game_2 cursorController;
    AttackAreaRectangle attackAreaRectangle;

    protected override void Awake()
    {
        cursorController = GetComponentInChildren<CursorController_Game_2>(true);
        attackAreaRectangle = GetComponentInChildren<AttackAreaRectangle>(true);
    }

    public override void StartFishing()
    {
        base.StartFishing();
        
        game123.alpha = 1;
        game4.alpha = 0;

        FishController.Instance.Setting(this, (FishingMethodData_Game_1)FishingData.MethodData);

        cursorController.Setting();

        attackAreaRectangle.Setting();
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
