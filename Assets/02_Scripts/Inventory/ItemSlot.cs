using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image itemIconImage;
    [SerializeField] GameObject baitSelectImage;
    [SerializeField] TMP_Text countText;
    [SerializeField] CanvasGroup canvasGroup;

    [SerializeField] bool isAlwayView = true;

    Item item = null;

    public void Clear()
    {
        canvasGroup.alpha = isAlwayView ? 1 : 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        baitSelectImage.SetActive(false);
        countText.text = string.Empty;
        if (gameObject.activeSelf && !isAlwayView) gameObject.SetActive(false);

        itemIconImage.color = new Color(1, 1, 1, 0);
    }
    public void Setting(Item item)
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
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ContextMenuManager.Instance.OpenMenu(item, eventData.position);
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            ExplainText.Instance.Setting(item.itemExplain);
        }
    }
}
