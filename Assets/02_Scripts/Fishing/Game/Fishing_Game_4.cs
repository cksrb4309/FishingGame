public class Fishing_Game_4 : Fishing_Game
{
    public override void StartFishing()
    {
        base.StartFishing();

        SetCanvas(gameIndex: 4);
        
        UserMainController.Instance.Setting();
        FishPatternController.Instance.Setting();
    }
    public override void CancelFishing()
    {
        base.CancelFishing();

        UserMainController.Instance.Cancel();
        FishPatternController.Instance.Cancel();
    }
    public override void Complete()
    {
        base.Complete();
    }
}
