using System;
using System.Collections;
using UnityEngine;
using VInspector;

public class PlayerFishingLineController : MonoBehaviour, IDirectionable
{
    public static PlayerFishingLineController Instance { get; private set; } = null;

    [SerializeField] SerializedDictionary<PlayerDir, Vector3> directionSettings;

    [SerializeField] GameObject bobberObject;
    [SerializeField] Transform bobberTransform = null;

    [SerializeField] Transform rodPivotTransform;
    [SerializeField] Transform rodTipTransform;

    [SerializeField] Transform lineRendererTransform;
    [SerializeField] LineRenderer lineRenderer;

    [SerializeField] int linePositionCount;

    Coroutine lineCoroutine = null;

    Vector3[] linePositions;

    bool isShow = false;

    IEnumerator LineCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.05f);

        bobberObject.SetActive(true);
        lineRenderer.gameObject.SetActive(true);

        Vector3 tipPosition = bobberTransform.position;

        while (true)
        {
            while (bobberTransform == null) yield return null;

            Vector3 pivotToTip = bobberTransform.position - lineRendererTransform.position;

            float horizontalRatio = Mathf.Abs(pivotToTip.x) / pivotToTip.magnitude;
            float maxGravityBend = 0.3f;
            float gravityBendAmount = horizontalRatio * maxGravityBend;

            for (int i = 0; i < linePositionCount; i++)
            {
                float t = (float)i / (linePositionCount - 1);

                Vector3 basePos = Vector3.Lerp(Vector3.zero, pivotToTip, t);

                float bend = Mathf.Sin(t * Mathf.PI); 
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