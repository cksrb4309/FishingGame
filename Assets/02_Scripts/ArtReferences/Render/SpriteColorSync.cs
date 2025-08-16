using DG.Tweening;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteColorSync : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private MaterialPropertyBlock propertyBlock;

    private static readonly int ColorID = Shader.PropertyToID("_Color"); // Shader Graph의 Color 프로퍼티 이름

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        propertyBlock = new MaterialPropertyBlock();
    }
    void LateUpdate()
    {
        propertyBlock.SetColor(ColorID, spriteRenderer.color);
        spriteRenderer.SetPropertyBlock(propertyBlock);
    }
}