using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] protected Image itemIconImage;
    [SerializeField] protected GameObject baitSelectImage;
    [SerializeField] protected TMP_Text countText;
    [SerializeField] protected CanvasGroup canvasGroup;

    [SerializeField] protected bool isAlwayView = true;

    protected Item item = null;

    public virtual void Clear()
    {
        canvasGroup.alpha = isAlwayView ? 1 : 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        baitSelectImage.SetActive(false);
        countText.text = string.Empty;
        if (gameObject.activeSelf && !isAlwayView) gameObject.SetActive(false);

        itemIconImage.color = new Color(1, 1, 1, 0);
    }
    public virtual void SettingItem(Item item)
    {
        this.item = item;

        if (!gameObject.activeSelf && !isAlwayView) gameObject.SetActive(true);
        canvasGroup.alpha = 1f;

        itemIconImage.sprite = item.itemIcon;
        itemIconImage.color = Color.white;
        countText.text = item.itemCount.ToString();

        baitSelectImage.SetActive(item.isUsedAsBait);

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        switch (PlayerInventory.Instance.inventoryState)
        {
            case InventoryState.None:

                if (eventData.button == PointerEventData.InputButton.Right)
                {
                    ContextMenuManager.Instance.OpenMenu(item, eventData.position);
                }
                else if (eventData.button == PointerEventData.InputButton.Left)
                {
                    ExplainText.Instance.Setting(item);
                }

                break;

            case InventoryState.Shop:

                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    ShopExplainText.Instance.Setting(item);

                    ShopSellButton.Instance.SelectItem(item);
                }

                break;
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ContextMenuManager.Instance.OpenMenu(item, eventData.position);
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            ExplainText.Instance.Setting(item);
        }
    }
}
