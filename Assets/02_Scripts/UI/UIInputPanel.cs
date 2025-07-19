using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputPanel : UIPanel
{
    [SerializeField] InputType inputType;
    InputActionReference inputAction = null;
    protected void Update()
    {
        if (inputAction.action.WasPressedThisFrame()) Show();
    }
    protected virtual void OnEnable()
    {
        inputAction = InputManager.GetInputAction(inputType);
    }
    protected override void OnDisable()
    {
        InputManager.Release(inputType);
    }
}
