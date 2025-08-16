using UnityEngine;

public class Fishing_Game_1 : Fishing_Game
{
    protected override void Awake()
    {
        base.Awake();

        cursorController = GetComponentInChildren<CursorController_Game_1>(true);
    }
    public override void StartFishing()
    {
        base.StartFishing();

        SetCanvas(gameIndex: 1);

        fishController.Setting((FishingMethodData_Game_5)FishingData.MethodData);

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
