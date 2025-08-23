using System;
using UnityEngine;
using static VFavorites.VFavoritesState;

public class GlobalStateObserver
{
    public static bool IsFishing { get; private set; } = false;
    private static Action<bool> fishingStateOnChanged = null;

    public static void NotifyFishingStateChanged(bool isFishing)
    {
        IsFishing = isFishing;

        fishingStateOnChanged?.Invoke(isFishing);
    }
    public static void FishingStateActionSubscribe(Action<bool> callback)
    {
        fishingStateOnChanged += callback;
    }
    public static void FishingStateActionUnsubscribe(Action<bool> callback)
    {
        fishingStateOnChanged -= callback;
    }
}
