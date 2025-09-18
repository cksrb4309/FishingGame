using UnityEngine;

[ExecuteAlways]
public class LetterboxUI : MonoBehaviour
{
    public float targetWidth = 16f;
    public float targetHeight = 9f;

    public RectTransform top;
    public RectTransform bottom;
    public RectTransform left;
    public RectTransform right;

    void Update()
    {
        float screenAspect = (float)Screen.width / Screen.height;
        float targetAspect = targetWidth / targetHeight;

        if (screenAspect > targetAspect)
        {
            // ÁÂ¿ì ¿©¹é »ý±è (¼¼·Î ¸ÂÃã)
            float targetWidthInPixels = Screen.height * targetAspect;
            float sideWidth = (Screen.width - targetWidthInPixels) / 2f;

            left.sizeDelta = new Vector2(sideWidth, 0);
            right.sizeDelta = new Vector2(sideWidth, 0);

            left.gameObject.SetActive(true);
            right.gameObject.SetActive(true);
            top.gameObject.SetActive(false);
            bottom.gameObject.SetActive(false);
        }
        else
        {
            // »óÇÏ ¿©¹é »ý±è (°¡·Î ¸ÂÃã)
            float targetHeightInPixels = Screen.width / targetAspect;
            float topBottomHeight = (Screen.height - targetHeightInPixels) / 2f;

            top.sizeDelta = new Vector2(0, topBottomHeight);
            bottom.sizeDelta = new Vector2(0, topBottomHeight);

            top.gameObject.SetActive(true);
            bottom.gameObject.SetActive(true);
            left.gameObject.SetActive(false);
            right.gameObject.SetActive(false);
        }
    }
}
