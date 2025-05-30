using UnityEngine;

public class Fishing_AutoComplete : MonoBehaviour, IFishingSystem
{
    [SerializeField] Transform bobberTransform;

    Item targetItem = null;

    public void StartFishing(Item targetItem)
    {
        this.targetItem = targetItem;

        Complete();
    }
    public void CancelFishing()
    {

    }
    public void Complete()
    {
        Debug.Log("È¹µæ : " + targetItem.itemName);

        PlayerFishingManager.Instance.Complete();
    }
}
