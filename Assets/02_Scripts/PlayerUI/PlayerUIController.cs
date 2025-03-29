using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [Header("���¹̳� UI")]
    [SerializeField] CanvasGroup staminaCanvasGroup;
    [SerializeField] Image staminaFillImage;
    [SerializeField] float staminaHideTime = 1f;
    [SerializeField] float staminaFadeSpeed = 1f;

    float staminaViewTime = 10f;
    public void Start()
    {
        staminaCanvasGroup.alpha = 0f;

        StartCoroutine(StaminaCoroutine());
    }
    IEnumerator StaminaCoroutine()
    {
        while (true)
        {
            if (staminaViewTime < staminaHideTime)

                staminaCanvasGroup.alpha += Time.deltaTime * staminaFadeSpeed;
            
            else staminaCanvasGroup.alpha -= Time.deltaTime * staminaFadeSpeed;

            staminaViewTime += Time.deltaTime;

            yield return null;
        }
    }
    public void SetStaminaAmount(float amount)
    {
        staminaFillImage.fillAmount = amount;

        staminaViewTime = 0f;
    }
}
