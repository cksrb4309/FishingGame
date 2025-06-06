using UnityEngine;

public class Fishing_AutoComplete : MonoBehaviour, IFishingSystem
{
    [SerializeField] Transform bobberTransform;

    public void StartFishing()
    {
        Complete();
    }
    public void CancelFishing()
    {

    }
    public void Complete()
    {
        Debug.Log("È¹µæ : " + FishingData.TargetItem.itemName);

        PlayerFishingManager.Instance.Complete();
    }
}
