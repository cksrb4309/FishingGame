using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InGameUIBackgroundPanel : UIPanel
{
    [SerializeField] RawImage blurImage;
    public override void Show(bool trigger = true)
    {
        Debug.Log("InGameUIBackgroundPanel Show");
        if (trigger) UIManager.OnShow(this);

        BackgroundSetting(true);
    }
    public override void Hide(bool trigger = true)
    {
        Debug.Log("InGameUIBackgroundPanel Hide");

        if (trigger) UIManager.OnHide(this);

        BackgroundSetting(false);
    }
    void BackgroundSetting(bool isShow)
    {
        Debug.Log("BackgroundSetting : " + isShow.ToString());

        canvasGroup.DOKill();
        blurImage.material.DOKill();

        canvasGroup.DOFade(isShow ? 1f : 0f, 0.2f);
        blurImage.material.DOFloat(isShow ? 1f : 0f, "_Alpha", 0.2f);

        canvasGroup.interactable = isShow;
        canvasGroup.blocksRaycasts = isShow;
    }
}
