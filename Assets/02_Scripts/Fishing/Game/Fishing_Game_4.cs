public class Fishing_Game_4 : Fishing_Game
{
    public override void StartFishing()
    {
        base.StartFishing();

        game123.alpha = 0;
        game4.alpha = 1;
        
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
