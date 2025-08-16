using TMPro;
using UnityEngine;

public class ShopExplainText : MonoBehaviour
{
    public static ShopExplainText Instance { get; private set; } = null;

    TMP_Text textUI;

    private void Awake()
    {
        Instance = this;

        textUI = GetComponent<TMP_Text>();
    }

    public void Setting(Item item)
    {
        if (item != null)
        {
            textUI.text = item.itemExplain;
            textUI.text += "\n\n";
            textUI.text += $"판매가 : {item.itemPrice}원";
        }
        else
        {
            textUI.text = string.Empty;
        }
    }
}
