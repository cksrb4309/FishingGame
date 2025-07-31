using UnityEngine;

public class Fishing_Game_1 : Fishing_Game
{
    CursorController_Game_1 cursorController;
    protected override void Awake()
    {
        base.Awake();

        cursorController = GetComponentInChildren<CursorController_Game_1>(true);
    }
    public override void StartFishing()
    {
        base.StartFishing();

        SetCanvas(gameIndex: 1);

        FishController_New.Instance.Setting((FishingMethodData_Game_1)FishingData.MethodData);

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
