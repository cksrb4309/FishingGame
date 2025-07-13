using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InGameUIBackgroundPanel : UIInputPanel
{
    [SerializeField] RawImage blurImage;
    public override void Show(bool trigger = true)
    {
        if (trigger) UIManager.OnShow(this);

        BackgroundSetting(true);
    }
    public override void Hide(bool trigger = true)
    {
        if (trigger) UIManager.OnHide(this);

        BackgroundSetting(false);
    }
    void BackgroundSetting(bool isShow)
    {
        if (isShow) PlayerFishingManager.Instance.DisableFishing();
        else PlayerFishingManager.Instance.EnableFishing();

        canvasGroup.DOKill();
        blurImage.material.DOKill();

        canvasGroup.DOFade(isShow ? 1f : 0f, 0.2f);
        blurImage.material.DOFloat(isShow ? 1f : 0f, "_Alpha", 0.2f);

        canvasGroup.interactable = isShow;
        canvasGroup.blocksRaycasts = isShow;
    }
}
