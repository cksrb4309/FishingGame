using TMPro;
using UnityEngine;

public class ExplainText : MonoBehaviour
{
    public static ExplainText Instance { get; private set; } = null;

    TMP_Text textUI;

    private void Awake()
    {
        Instance = this;

        textUI = GetComponent<TMP_Text>();
    }

    public void Setting(Item item)
    {
        textUI.text = item != null ? item.itemExplain : string.Empty;
    }
}
