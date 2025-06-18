using System;
using UnityEngine;
using DG.Tweening;

[ExecuteAlways]
public class RenderMaterialController : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] float startAlpha = 0f;
    [SerializeField] CanvasGroup canvasGroup = null;
    private static readonly int AlphaID = Shader.PropertyToID("_Alpha"); // Shader Graph의 Color 프로퍼티 이름
    public void Fade(float endValue, float duration, Action action = null)
    {
        // TODO : 렌더링 문제 생기면 이쪽 확인
        material.DOFloat(endValue, AlphaID, duration).OnComplete(() => { action?.Invoke(); });
        if (canvasGroup != null) canvasGroup.DOFade(endValue, duration);
    }
    private void Start()
    {
        material.SetFloat(AlphaID, startAlpha);
    }
}