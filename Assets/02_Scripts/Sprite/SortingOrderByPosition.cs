using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SortingOrderByPosition : MonoBehaviour
{
    [Tooltip("SortingOrder에 더할 오프셋 값")]
    public int sortingOrderOffset = 0;

    [Tooltip("Y 좌표 기준으로 SortingOrder를 계산하는데 곱하는 값")]
    public float sortingOrderMultiplier = 100f;

    [Tooltip("이 오브젝트가 동적일 경우 true, 정적일 경우 false")]
    public bool isDynamic = true;

    private SpriteRenderer spriteRenderer;

    // 이전 Y 위치 (변화 감지용)
    private float lastY;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastY = transform.position.y;
        UpdateSortingOrder();
    }

    private void Update()
    {
        if (!isDynamic) return;

        // Y 위치가 충분히 변했을 때만 업데이트 (불필요한 연산 방지)
        float currentY = transform.position.y;
        if (Mathf.Abs(currentY - lastY) > 0.01f)
        {
            UpdateSortingOrder();
            lastY = currentY;
        }
    }

    private void UpdateSortingOrder()
    {
        // Y 좌표를 기준으로 sortingOrder 계산 (아래가 클수록 앞에 보이게)
        int order = Mathf.RoundToInt(-transform.position.y * sortingOrderMultiplier) + sortingOrderOffset;
        spriteRenderer.sortingOrder = order;
    }
}
