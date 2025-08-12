using UnityEngine;
using UnityEngine.EventSystems;

public class QuestItemSlot : ItemSlot
{
    private void OnDisable()
    {
        if (item != null)
        {
            item = null;

            canvasGroup.alpha = 0f;
        }
    }
    public override void Clear()
    {
        canvasGroup.alpha = 0f;
        countText.text = string.Empty;
        itemIconImage.color = new Color(1, 1, 1, 0);
    }
    public void SettingQuestItem(Item item, int itemCount)
    {
        this.item = item;

        canvasGroup.alpha = 1f;

        itemIconImage.sprite = item.itemIcon;
        countText.text = itemCount.ToString();

        //canvasGroup.interactable = true;
        //canvasGroup.blocksRaycasts = true;
    }
    public override void SettingItem(Item item)
    {

    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        // 퀘스트 아이템 UI 클릭 시 작용
    }
}
