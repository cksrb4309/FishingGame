using UnityEngine;
using VInspector;

public class FishingGameController : MonoBehaviour
{
    public static FishingGameController Instance { get; private set; } = null;

    [SerializeField] SerializedDictionary<FishingStyle, Fishing_Game> fishingGames = new();

    FishingStyle currentGameStyle;
    Fishing_Game Current_FishingGame => fishingGames[currentGameStyle];

    public void StartFishing(FishingStyle fishingStyle)
    {
        currentGameStyle = fishingStyle;

        if (!Current_FishingGame.gameObject.activeSelf) Current_FishingGame.gameObject.SetActive(true);

        Current_FishingGame.StartFishing();
    }

    public void CancelFishing()
    {
        Current_FishingGame.CancelFishing();
    }

    public void CompleteFishing()
    {
        Current_FishingGame.Complete();

        CancelFishing();
    }

    private void Awake()
    {
        Instance = this;
    }
}
