using TMPro;
using UnityEngine;

public class ExplainText : MonoBehaviour
{
    public static ExplainText Instance { get; private set; } = null;

    [SerializeField] TMP_Text textUI;

    private void Awake()
    {
        Instance = this;
    }

    public void Setting(string explainText)
    {
        textUI.text = explainText;
    }
}
