using UnityEngine;

public class Fishing_Game_2 : MonoBehaviour, IFishingSystem 
{
    CursorController_Game_2 cursorController;
    RenderMaterialController renderMaterialController;
    AttackAreaRectangle attackAreaRectangle;

    private void Awake()
    {
        cursorController = GetComponentInChildren<CursorController_Game_2>(true);
        attackAreaRectangle = GetComponentInChildren<AttackAreaRectangle>(true);

        renderMaterialController = GetComponentInParent<RenderMaterialController>();
    }

    public void StartFishing()
    {
        FishController.Instance.Setting(this, (FishingMethodData_Game_1)FishingData.MethodData);

        cursorController.Setting();

        attackAreaRectangle.Setting();

        renderMaterialController.Fade(1f, 0.5f);
    }
    public void CancelFishing()
    {
        FishController.Instance.Cancel();
        cursorController.Cancel();

        renderMaterialController.Fade(0, 0.5f);
    }
    public void Complete()
    {
        CancelFishing();

        PlayerFishingManager.Instance.Complete();
    }
}
