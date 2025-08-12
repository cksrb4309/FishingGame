using System;
using System.Collections;
using UnityEngine;
using VInspector;

public class PlayerFishingRodController : MonoBehaviour, IDirectionable
{
    [SerializeField] public SerializedDictionary<PlayerDir, Vector3> directionSettings;

    [SerializeField] Transform rodTipTransform;

    void SetTipPosition(Vector3 position)
    {
        rodTipTransform.localPosition = position;
    }
    public void UpdateDirection(PlayerDir dir)
    {
        SetTipPosition(directionSettings[dir]);
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
