using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishingZone", menuName = "Fishing/FishingZone")]
public class FishingZone : ScriptableObject
{
    public List<Item> items;
}
