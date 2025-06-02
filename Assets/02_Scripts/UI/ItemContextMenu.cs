using UnityEngine;

public class ItemContextMenu : MonoBehaviour
{
    [SerializeField] ItemCategory category;

    Item item = null;
    
    public void Setup(Item item)
    {
        if ((item.category & category) > 0)
        {
            this.item = item;

            if (!gameObject.activeSelf) gameObject.SetActive(true);
        }
        else CleanUp();
    }
    void CleanUp()
    {
        item = null;

        if (gameObject.activeSelf) gameObject.SetActive(false);
    }
    public void SelectBait()
    {
        PlayerInventory.Instance.SelectBaitItem(item);

        ContextMenuManager.Instance.CloseMenu();
    }
}
