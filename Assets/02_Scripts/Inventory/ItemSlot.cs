using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] Image itemIconImage;
    [SerializeField] TMP_Text countText;

    public void Clear()
    {
        gameObject.SetActive(false);

        itemIconImage.sprite = null;
        itemIconImage.gameObject.SetActive(false);

        countText.text = "";
        countText.gameObject.SetActive(false);
    }
    public void Setting(Item item)
    {
        if (item.itemCount > 0)
        {
            if (!gameObject.activeSelf) gameObject.SetActive(true);

            itemIconImage.sprite = item.itemIcon;
            countText.text = item.itemCount.ToString();
        }
    }
}
