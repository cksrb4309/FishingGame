using System;
using System.Collections;
using UnityEngine;
using VInspector;

public class PlayerFishingRodController : MonoBehaviour, IDirectionable
{
    [SerializeField] public SerializedDictionary<PlayerDir, FishingDirSet> directionSettings;

    [SerializeField] Transform rodPivotTransform;
    [SerializeField] Transform rodTipTransform;

    [SerializeField] LineRenderer rodLineRenderer;

    [SerializeField] int rodLinePositionCount;

    Vector3[] rodPositions;

    [SerializeField] float rodTension = 0f;

    Coroutine rodTensionCoroutine = null;

    IEnumerator RodTensionCoroutine(FishingDirSet set)
    {
        Vector3 tipPosition = set.tipPosition - set.pivotPosition;
        Vector3 tensionOffset = set.defaultTensionPosition;

        while (true)
        {
            for (int i = 0; i < rodLinePositionCount; i++)
            {
                float t = (float)i / (rodLinePositionCount - 1);

                //// 기본 선형 위치
                //Vector3 basePos = Vector3.Lerp(Vector3.zero, tipPosition, t);

                //// 휘어짐 효과: Sin(t * PI) 은 0 → 1 → 0으로 부드러운 휘어짐
                //Vector3 curvedOffset = Mathf.Sin(t * Mathf.PI) * tensionOffset * rodTension;

                //// 최종 위치
                //rodPositions[i] = basePos + curvedOffset;

                rodPositions[i] = Vector3.Lerp(Vector3.zero, tipPosition, t);
            }

            rodLineRenderer.SetPositions(rodPositions);

            yield return null;
        }
    }
    void SetDirection(FishingDirSet set)
    {


        rodPivotTransform.localPosition = set.pivotPosition;
        rodTipTransform.localPosition = set.tipPosition;
        if (rodTensionCoroutine != null) StopCoroutine(rodTensionCoroutine);
        rodTensionCoroutine = StartCoroutine(RodTensionCoroutine(set));
    }
    public void UpdateDirection(PlayerDir dir)
    {
        SetDirection(directionSettings[dir]);
    }
    private void Awake()
    {
        rodPositions = new Vector3[rodLinePositionCount];

        rodLineRenderer.positionCount = rodLinePositionCount;
    }
    void EditFishingDirSet()
    {
        //foreach (FishingDirSet set in directionSettings.Values) (set.pivotPosition, set.tipPosition) = (Vector3.zero, set.tipPosition - set.pivotPosition);
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
public class FishingDirSet
{
    public Vector3 pivotPosition;
    public Vector3 tipPosition;
    public Vector3 defaultTensionPosition;
    public float angle;
}