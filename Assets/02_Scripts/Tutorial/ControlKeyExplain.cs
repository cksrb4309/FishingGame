using DG.Tweening;
using UnityEngine;

public class ControlKeyExplain : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float startFadeDuration = 0;
    [SerializeField] float startFadeDelay = 0;
    [SerializeField] float endFadeDuration = 1f;
    [SerializeField] float endFadeDelay = 2f;

    private void Start()
    {
        canvasGroup.DOFade(1f, startFadeDuration).SetDelay(startFadeDelay).OnComplete(() => canvasGroup.DOFade(0f, endFadeDuration).SetDelay(endFadeDelay));
    }
}
