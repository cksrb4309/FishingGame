using DG.Tweening;
using System;
using UnityEngine;
using VInspector;

public class PlayerDirectionTestViewer : MonoBehaviour, IDirectionable
{
    [SerializeField] SerializedDictionary<PlayerDir, TestDirSet> directionSettings;
    [SerializeField] Transform image;
    void SetDirection(TestDirSet set)
    {
        image.DOKill();
        image.transform.DOLocalMove(set.targetTransform.localPosition, 0.1f);
    }
    public void UpdateDirection(PlayerDir dir)
    {
        SetDirection(directionSettings[dir]);
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
public class TestDirSet
{
    public Transform targetTransform;
}