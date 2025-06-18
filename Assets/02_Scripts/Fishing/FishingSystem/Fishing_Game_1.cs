using DG.Tweening;
using UnityEngine;

public class Fishing_Game_1 : MonoBehaviour, IFishingSystem // Ä«¼­½º Q °ø°Ý °ÔÀÓ
{
    [SerializeField] FishController fishController;
    [SerializeField] CursorController_Game_1 cursorController;
    [SerializeField] RenderMaterialController renderMaterialController;

    public void StartFishing()
    {
        fishController.Setting(this, (FishingMethodData_Game_1)FishingData.MethodData);
        cursorController.Setting();

        renderMaterialController.Fade(1f, 0.5f);
    }
    public void CancelFishing()
    {
        fishController.Cancel();
        cursorController.Cancel();

        renderMaterialController.Fade(0f, 0.5f);
    }
    public void Complete()
    {
        CancelFishing();

        Debug.Log("È¹µæ : " + FishingData.TargetItem.itemName);

        PlayerFishingManager.Instance.Complete();
    }
}
