public class Fishing_Game_4 : Fishing_Game
{
    public override void StartFishing()
    {
        base.StartFishing();

        game123.alpha = 0;
        game4.alpha = 1;
        
        UserController.Instance.Setting();
        FishPatternController.Instance.Setting();
    }
    public override void CancelFishing()
    {
        base.CancelFishing();

        UserController.Instance.Cancel();
        FishPatternController.Instance.Cancel();
    }
    public override void Complete()
    {
        base.Complete();
    }
}
