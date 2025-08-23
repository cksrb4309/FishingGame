using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
[ExecuteAlways]
public class DynamicGridResizer : MonoBehaviour
{
    public RectTransform targetRect;  // 보통은 Viewport나 Content의 부모
    public int columnCount = 5;
    public float spacing = 5f;
    public float padding = 10f;

    private GridLayoutGroup grid;

    void Awake()
    {
        grid = GetComponent<GridLayoutGroup>();
    }

    void Update()
    {
        float totalSpacing = spacing * (columnCount - 1);
        float totalPadding = padding * 2f;
        float width = targetRect.rect.width;

        float cellWidth = (width - totalSpacing - totalPadding) / columnCount;
        grid.cellSize = new Vector2(cellWidth, cellWidth); // 정사각형 슬롯
    }
}