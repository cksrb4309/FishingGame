using UnityEngine;

public class Fishing_Game_3 : MonoBehaviour, IFishingSystem 
{
    CursorController_Game_3 cursorController;
    RenderMaterialController renderMaterialController;

    private void Awake()
    {
        cursorController = GetComponentInChildren<CursorController_Game_3>(true);
        renderMaterialController = GetComponentInParent<RenderMaterialController>();
    }
    private void Start()
    {
        PoolManager.CreatePool<AttackProjectile>(ObjectPoolID.AttackProjectile, 4);
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

        renderMaterialController.Fade(0, 0.5f);
    }
    public void Complete()
    {
        CancelFishing();
        
        PlayerFishingManager.Instance.Complete();
    }
}
