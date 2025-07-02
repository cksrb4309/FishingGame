public class Fishing_Game_5 : Fishing_Game
{
    CursorController_Game_5 cursorController;
    protected override void Awake()
    {
        base.Awake();

        cursorController = GetComponentInChildren<CursorController_Game_5>(true);
    }
    public override void StartFishing()
    {
        base.StartFishing();

        SetCanvas(gameIndex: 5);

        FishController_New.Instance.Setting((FishingMethodData_Game_5)FishingData.MethodData);

        cursorController.Setting();
    }
    public override void CancelFishing()
    {
        base.CancelFishing();

        cursorController.Cancel();
    }
    public override void Complete()
    {
        base.Complete();
    }
}
