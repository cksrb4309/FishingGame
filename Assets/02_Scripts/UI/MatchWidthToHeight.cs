using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class MatchWidthToHeight : MonoBehaviour
{
    public float aspectRatio = 1.0f; // width = height * aspectRatio

    [SerializeField] RectTransform.Axis axis = RectTransform.Axis.Horizontal;

    private RectTransform rect;

    void Update()
    {
        if (rect == null) rect = GetComponent<RectTransform>();

        if (axis == RectTransform.Axis.Horizontal)
        {
            float height = rect.rect.height;
            rect.SetSizeWithCurrentAnchors(axis, height * aspectRatio);
        }
        else
        {
            float width = rect.rect.width;
            rect.SetSizeWithCurrentAnchors(axis, width * aspectRatio);
        }
    }
}
