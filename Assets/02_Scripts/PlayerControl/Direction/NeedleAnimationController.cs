using System;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class NeedleAnimationController : MonoBehaviour, IDirectionable
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] SpriteRenderer needleRenderer;

    [SerializeField] SerializedDictionary<PlayerDir, List<NeedleSet>> needleSetDictionary;

    List<NeedleSet> currentNeedleSet;
    int currentIndex = 0;

    public void UpdateDirection(PlayerDir dir)
    {
        currentNeedleSet = needleSetDictionary[dir];

        needleRenderer.flipX = dir == PlayerDir.TopRight || dir == PlayerDir.BottomLeft;

        Apply();
    }
    public void NeedleSetting(int index)
    {
        currentIndex = index;

        Apply();
    }
    void Apply()
    {
        //lineRenderer.positionCount = currentNeedleSet[currentIndex].linePositions.Length;
        lineRenderer.SetPositions(currentNeedleSet[currentIndex].linePositions);

        needleRenderer.transform.localPosition = currentNeedleSet[currentIndex].position;
        needleRenderer.transform.rotation = Quaternion.Euler(0f, 0f, currentNeedleSet[currentIndex].angle);
    }
    void SetIsFishing(bool isFishing)
    {
        lineRenderer.gameObject.SetActive(isFishing);
        needleRenderer.gameObject.SetActive(isFishing);
    }
    private void Awake()
    {
        currentNeedleSet = needleSetDictionary[PlayerDir.BottomRight];
    }
    private void OnEnable()
    {
        GlobalStateObserver.FishingStateActionSubscribe(SetIsFishing);
        DirectionManager.Instance.RegisterDirectionable(this);
    }
    private void OnDisable()
    {
        GlobalStateObserver.FishingStateActionUnsubscribe(SetIsFishing);
        DirectionManager.Instance.UnregisterDirectionable(this);
    }
}

[Serializable]
public class NeedleSet
{
    public float angle;
    public Vector2 position;

    public Vector3[] linePositions;
}