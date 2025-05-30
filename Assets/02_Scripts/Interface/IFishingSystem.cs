public interface IFishingSystem
{
    public void StartFishing(Item targetItem);
    public void Complete();
    public void CancelFishing();
}