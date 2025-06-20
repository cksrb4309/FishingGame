using DG.Tweening;
using UnityEngine;

public class Fishing_Game_1 : MonoBehaviour, IFishingSystem
{
    CursorController_Game_1 cursorController;
    RenderMaterialController renderMaterialController;

    private void Awake()
    {
        cursorController = GetComponentInChildren<CursorController_Game_1>(true);
        renderMaterialController = GetComponentInParent<RenderMaterialController>();
    }
    public void StartFishing()
    {
        FishController.Instance.Setting(this, (FishingMethodData_Game_1)FishingData.MethodData);
        cursorController.Setting();

        renderMaterialController.Fade(1f, 0.5f);
    }
    public void CancelFishing()
    {
        FishController.Instance.Cancel();
        cursorController.Cancel();

        renderMaterialController.Fade(0f, 0.5f);
    }
    public void Complete()
    {
        CancelFishing();
        
        PlayerFishingManager.Instance.Complete();
    }
}
