using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VInspector;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager Instance { get; private set; } = null;

    [SerializeField] RawImage blurImage;
    [SerializeField] CanvasGroup uiCanvasGroup;

    [SerializeField] SerializedDictionary<InGameUIState, GameObject> inGameUIPannels;

    InputActionReference inventoryOpenInputAction;

    InGameUIState currentState = InGameUIState.None;
    public void SelectUI(InGameUIState nextState)
    {
        if (currentState != nextState) ChangeUI(nextState);
        else BackgroundSetting(false);
    }
    void ChangeUI(InGameUIState nextState)
    {
        if (currentState == InGameUIState.None)
            BackgroundSetting(true);
        
        foreach (var state in inGameUIPannels.Keys)
            inGameUIPannels[state].SetActive(state == nextState);
    }
    void BackgroundSetting(bool isShow)
    {
        uiCanvasGroup.DOKill();
        blurImage.material.DOKill();
        uiCanvasGroup.DOFade(isShow ? 1f : 0f, 0.3f);
        blurImage.material.DOFloat(isShow ? 1f : 0f, "_Alpha", 0.3f);
        if (isShow == false) currentState = InGameUIState.None;
        uiCanvasGroup.interactable = isShow;
        uiCanvasGroup.blocksRaycasts = isShow;
    }
    private void Update()
    {
        if (inventoryOpenInputAction.action.WasPressedThisFrame())
            SelectUI(InGameUIState.Inventory);
    }
    private void OnEnable()
    {
        inventoryOpenInputAction = InputManager.GetInputAction(InputType.Inventory);
    }
    private void OnDisable()
    {
        InputManager.Release(InputType.Inventory);
    }
}
