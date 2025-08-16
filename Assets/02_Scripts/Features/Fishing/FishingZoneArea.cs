using UnityEngine;

public class FishingZoneArea : MonoBehaviour
{
    [SerializeField] FishingZone zone;
    public FishingZone GetFishingZone() => zone;
}
