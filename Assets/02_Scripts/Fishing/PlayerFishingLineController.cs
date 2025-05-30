using System;
using System.Collections;
using UnityEngine;
using VInspector;

public class PlayerFishingLineController : MonoBehaviour, IDirectionable
{
    public static PlayerFishingLineController Instance { get; private set; } = null;

    [SerializeField] SerializedDictionary<PlayerDir, Vector3> directionSettings;

    [SerializeField] GameObject bobberObject;

    [SerializeField] Transform rodPivotTransform;
    [SerializeField] Transform rodTipTransform;

    [SerializeField] Transform lineRendererTransform;
    [SerializeField] LineRenderer lineRenderer;

    [SerializeField] int linePositionCount;

    [SerializeField] Transform targetTransform = null;

    Coroutine lineCoroutine = null;

    Vector3[] linePositions;

    bool isShow = false;

    IEnumerator LineCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.01f);

        bobberObject.SetActive(true);
        lineRenderer.gameObject.SetActive(true);

        Vector3 tipPosition = targetTransform.position;

        while (true)
        {
            while (targetTransform == null) yield return null;

            Vector3 pivotToTip = targetTransform.position - lineRendererTransform.position;

            float horizontalRatio = Mathf.Abs(pivotToTip.x) / pivotToTip.magnitude;
            float maxGravityBend = 0.3f;
            float gravityBendAmount = horizontalRatio * maxGravityBend;

            for (int i = 0; i < linePositionCount; i++)
            {
                float t = (float)i / (linePositionCount - 1);

                // 기본 직선 위치
                Vector3 basePos = Vector3.Lerp(Vector3.zero, pivotToTip, t);

                // 중력 방향 휘어짐 (2D 탑뷰니까 Y축이 아래임)
                float bend = Mathf.Sin(t * Mathf.PI); // 중간이 가장 많이 휨
                Vector3 gravityOffset =  bend * gravityBendAmount * Vector3.down;

                linePositions[i] = basePos + gravityOffset;
            }
            lineRenderer.SetPositions(linePositions);

            yield return null;
        }
    }
    public void UpdateDirection(PlayerDir dir)
    {
        lineRendererTransform.localPosition = Vector3.zero;

        if (isShow)
        {
            if (lineCoroutine != null) StopCoroutine(lineCoroutine);

            lineCoroutine = StartCoroutine(LineCoroutine());
        }
    }
    private void Awake()
    {
        Instance = this;

        linePositions = new Vector3[linePositionCount];

        lineRenderer.positionCount = linePositionCount;
    }
    public void EnableLine()
    {
        isShow = true;

        lineCoroutine = StartCoroutine(LineCoroutine());
    }
    public void DisableLine()
    {
        isShow = false;

        bobberObject.SetActive(false);
        lineRenderer.gameObject.SetActive(false);

        if (lineCoroutine != null) StopCoroutine(lineCoroutine);
    }
    private void OnEnable()
    {
        DirectionManager.Instance.RegisterDirectionable(this);
    }
    private void OnDisable()
    {
        DirectionManager.Instance.UnregisterDirectionable(this);
    }
}

[Serializable]
public class FishingLineSet
{
    public Vector3 pivotPosition;
}