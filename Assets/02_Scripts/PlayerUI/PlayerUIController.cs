using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [Header("캔버스")]
    [SerializeField] Transform canvasTransform;

    [Header("스태미나 UI")]
    [SerializeField] CanvasGroup staminaCanvasGroup;
    [SerializeField] Image staminaFillImage;
    [SerializeField] float staminaHideTime = 1f;
    [SerializeField] float staminaFadeSpeed = 1f;

    InputActionReference interactInputAction = null;

    float staminaViewTime = 10f;

    List<IInteractable> interactables = new();
    IInteractable interactable = null;
    public void Start()
    {
        staminaCanvasGroup.alpha = 0f;

        StartCoroutine(StaminaCoroutine());
    }
    public void LateUpdate()
    {
        //canvasTransform.position = transform.position;
    }
    public void Update()
    {
        if (interactInputAction.action.WasPressedThisFrame() && interactable != null)
            interactable.Interact();
    }
    IEnumerator StaminaCoroutine()
    {
        while (true)
        {
            staminaViewTime += Time.deltaTime;

            if (staminaViewTime < staminaHideTime)

                staminaCanvasGroup.alpha += Time.deltaTime * staminaFadeSpeed;
            
            else staminaCanvasGroup.alpha -= Time.deltaTime * staminaFadeSpeed;

            yield return null;
        }
    }
    public void SetStaminaAmount(float amount)
    {
        staminaFillImage.fillAmount = amount;

        staminaViewTime = 0f;
    }
    public void AddInteractable(IInteractable interactable)
    {
        if (!interactables.Contains(interactable))
        {
            interactables.Add(interactable);

            if (interactables.Count == 1)
            {
                this.interactable = interactable;
            }
            else
            {
                GetClosest();
            }
        }
    }
    void GetClosest()
    {
        IInteractable next = null;

        Vector3 a = transform.position;
        float distance = float.PositiveInfinity;

        foreach (IInteractable i in interactables)
        {
            Vector3 b = i.GetPosition();

            if (Vector3.Distance(a, b) < distance)
            {
                distance = Vector3.Distance(a, b);

                next = i;
            }
        }

        if (interactable != next)
        {
            if (interactable != null) interactable.Release();

            interactable = next;
        }
    }
    public void RemoveInteractable(IInteractable interactable)
    {
        if (interactables.Contains(interactable))
        {
            interactables.Remove(interactable);

            if (interactables.Count == 0)
            {
                this.interactable.Release();

                this.interactable = null;
            }
            else
            {
                GetClosest();
            }
        }
    }
    private void OnEnable()
    {
        interactInputAction = InputManager.GetInputAction(InputType.Interact);
    }
    private void OnDisable()
    {
        InputManager.Release(InputType.Interact);
    }
}
