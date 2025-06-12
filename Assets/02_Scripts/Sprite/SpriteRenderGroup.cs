using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpriteRenderGroup : MonoBehaviour
{
    [Range(0, 1f)][SerializeField] private float alpha = 1f;
    [SerializeField] CanvasGroup canvasGroup = null;
    public float Alpha
    {
        get => alpha;
        set
        {
            alpha = Mathf.Clamp01(value);
            ApplyAlpha();
            OnAlphaChanged?.Invoke(effectiveAlpha);
        }
    }
    public float effectiveAlpha { get; private set; } = 1f;
    readonly List<SpriteRenderer> renderers = new();
    SpriteRenderGroup parentGroup;
    public event Action<float> OnAlphaChanged;
    class RendererInfo
    {
        public SpriteRenderer renderer;
        public Color baseColor;
    }
    readonly List<RendererInfo> renderInfos = new();
    void Awake() => Init();
    void OnEnable() => Init();
    void OnTransformParentChanged() => Init();
#if UNITY_EDITOR
    void OnValidate() => Alpha = alpha;   // 에디터 슬라이더 반영
#endif
    void Init()
    {
        parentGroup = transform.parent ?
             transform.parent.GetComponentInParent<SpriteRenderGroup>() : null;

        if (parentGroup != null)   // 부모 알파 변경 이벤트 구독
            parentGroup.OnAlphaChanged += ParentChanged;

        RebuildRendererList();
        ApplyAlpha();
    }
    void OnDisable()
    {
        if (parentGroup != null)
            parentGroup.OnAlphaChanged -= ParentChanged;

        foreach (var info in renderInfos)
            if (info.renderer) info.renderer.color = info.baseColor;
    }
    void ParentChanged(float _) => ApplyAlpha();
    void RebuildRendererList()
    {
        renderers.Clear();
        CollectRenderers(transform);
    }
    void CollectRenderers(Transform root)
    {
        foreach (Transform t in root)
        {
            if (t == transform) continue;
            if (t.GetComponent<SpriteRenderGroup>()) continue;

            if (t.TryGetComponent(out SpriteRenderer sr))
                renderInfos.Add(new RendererInfo { renderer = sr, baseColor = sr.color });

            CollectRenderers(t);
        }
    }
    void ApplyAlpha()
    {
        effectiveAlpha = alpha * (parentGroup ? parentGroup.effectiveAlpha : 1f);
        if (canvasGroup != null) canvasGroup.alpha = effectiveAlpha;

        foreach (var info in renderInfos)
        {
            if (!info.renderer) continue;

#if UNITY_EDITOR
            Undo.RecordObject(info.renderer, "SpriteRenderGroup Change");
#endif
            var c = info.baseColor;              // **원본 값 복사**
            c.a = info.baseColor.a * effectiveAlpha;
            info.renderer.color = c;
        }
    }
    /*================ Fade ==================*/
    public void Fade(float endValue, float duration, Action onComplete = null)
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(endValue, duration, onComplete));
    }
    IEnumerator FadeRoutine(float endValue, float duration, Action onComplete)
    {
        float start = Alpha;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            Alpha = Mathf.Lerp(start, endValue, t / duration);
            if (canvasGroup != null) canvasGroup.alpha = Alpha;
            yield return null;
        }
        Alpha = endValue;
        if (canvasGroup != null) canvasGroup.alpha = Alpha;
        onComplete?.Invoke();
    }
}