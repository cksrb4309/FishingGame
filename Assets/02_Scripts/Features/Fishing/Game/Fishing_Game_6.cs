using UnityEngine;

public class Fishing_Game_6 : Fishing_Game
{
    protected override void Awake()
    {
        base.Awake();
    }
    public override void StartFishing()
    {
        base.StartFishing();

        SetCanvas(gameIndex: 6);

        fishController.Setting(FishingData.MethodData);

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
