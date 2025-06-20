using UnityEngine;

public class Fishing_Game_4 : MonoBehaviour, IFishingSystem 
{
    [SerializeField] UserController userController;
    RenderMaterialController renderMaterialController;

    private void Awake()
    {
        userController = GetComponentInChildren<UserController>(true);
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

        renderMaterialController.Fade(0, 0.5f);
    }
    public void Complete()
    {
        CancelFishing();
        
        PlayerFishingManager.Instance.Complete();
    }
}
