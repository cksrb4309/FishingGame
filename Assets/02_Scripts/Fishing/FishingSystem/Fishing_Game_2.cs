using DG.Tweening;
using UnityEngine;

public class Fishing_Game_2 : MonoBehaviour, IFishingSystem // º§ÄÚÁî R °ø°Ý °ÔÀÓ
{
    [SerializeField] FishController fishController;
    [SerializeField] CursorController_Game_2 cursorController;
    [SerializeField] CanvasGroup canvasGroup;

    public void StartFishing()
    {
        fishController.Setting(this, (FishingMethodData_Game_1)FishingData.MethodData);
        cursorController.Setting();

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
