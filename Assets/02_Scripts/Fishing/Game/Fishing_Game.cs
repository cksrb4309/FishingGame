using UnityEngine;

public class Fishing_Game : MonoBehaviour, IFishingSystem
{
    public static Fishing_Game fishing_Game = null;
    RenderMaterialController renderMaterialController;
    protected CanvasGroup game123 = null;
    protected CanvasGroup game4 = null;
    protected CanvasGroup game5 = null;
    protected virtual void Awake()
    {
        game123 = GameObject.Find("Fish_123").GetComponent<CanvasGroup>();
        game4 = GameObject.Find("Fish_User_4").GetComponent<CanvasGroup>();
        game5 = GameObject.Find("Hp_Game_5").GetComponent<CanvasGroup>();
        game123.alpha = 0f;
        game4.alpha = 0f;
        game5.alpha = 0f;

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
        Debug.Log("FishingGame Complete");
        PlayerFishingManager.Instance.Complete();
    }
    protected void SetCanvas(int gameIndex)
    {
        game123.alpha = gameIndex > 0 || gameIndex < 4 ? 1 : 0;
        game4.alpha = gameIndex == 4 ? 1 : 0;
        game5.alpha = gameIndex == 5 ? 1 : 0;
    }
}
