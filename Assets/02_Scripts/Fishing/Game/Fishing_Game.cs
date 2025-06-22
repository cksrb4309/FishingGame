using UnityEngine;

public class Fishing_Game : MonoBehaviour, IFishingSystem
{
    public static Fishing_Game fishing_Game = null;
    RenderMaterialController renderMaterialController;
    protected CanvasGroup game123 = null;
    protected CanvasGroup game4 = null;
    protected virtual void Awake()
    {
        game123 = GameObject.Find("Fish_123").GetComponent<CanvasGroup>();
        game4 = GameObject.Find("Fish_User_4").GetComponent<CanvasGroup>();
        game123.alpha = 0f;
        game4.alpha = 0f;
        
        renderMaterialController = GetComponentInParent<RenderMaterialController>();
    }
    public virtual void StartFishing()
    {
        renderMaterialController.Fade(1f, 0.5f);
        
        fishing_Game = this;
    }
    public virtual void CancelFishing()
    {
        renderMaterialController.Fade(0, 0.5f);
    }
    public virtual void Complete()
    {
        CancelFishing();
        
        PlayerFishingManager.Instance.Complete();
    }
}
