using DG.Tweening;
using UnityEngine;

public class Fishing_Game_1 : MonoBehaviour, IFishingSystem
{
    [SerializeField] FishController fishController;
    [SerializeField] FishingCursorController cursorController;
    [SerializeField] CanvasGroup canvasGroup;

    public void StartFishing()
    {
        fishController.Setting((FishingMethodData_Game_1)FishingData.MethodData);
        cursorController.Game_1_Setting();

        canvasGroup.DOFade(1f, 0.5f);
    }
    public void CancelFishing()
    {
        fishController.Cancel();
        cursorController.Cancel();

        canvasGroup.DOFade(0f, 0.5f);
    }
    public void Complete()
    {
        CancelFishing();

        Debug.Log("È¹µæ : " + FishingData.TargetItem.itemName);

        PlayerFishingManager.Instance.Complete();
    }
}
