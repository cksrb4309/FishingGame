using UnityEngine;
using UnityEngine.UI;

public class ShopSellButton : MonoBehaviour
{
    public static ShopSellButton Instance { get; private set; } = null;

    [SerializeField] Button button;

    Item selectedItem = null;

    private void Awake()
    {
        Instance = this;
    }

    public void SelectItem(Item item)
    {
        button.onClick.RemoveAllListeners();

        selectedItem = item;

        if (item == null || item.itemCount == 0)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;

            button.onClick.AddListener(SellItem);
        }
    }
    public void SellItem()
    {
        if (selectedItem == null || selectedItem.itemCount == 0) return;

        PlayerInventory.Instance.SellItem(selectedItem);

        SelectItem(selectedItem);
    }
}
