using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    [SerializeField] List<UIPanel> requiredPanels;
    public List<UIPanel> RequiredPanels => requiredPanels;
    protected CanvasGroup canvasGroup = null;
    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public virtual void Show(bool trigger = true)
    {
        if (trigger) UIManager.OnShow(this);

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    public virtual void Hide(bool trigger = true)
    {
        if (trigger) UIManager.OnHide(this);

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    protected virtual void OnDisable()
    {
        UIManager.OnHide(this);
    }
}
