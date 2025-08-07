using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VInspector;

public class PlayerFishingManager : MonoBehaviour
{
    public static PlayerFishingManager Instance { get; private set; } = null;

    [SerializeField] SerializedDictionary<FishingStyle, GameObject> fishingSystems = new();

    [SerializeField] LayerMask zoneLayerMask;

    [SerializeField] Item defaultBaitItem;

    [SerializeField] Transform bobberTransform;
    [SerializeField] Transform rodTipTransform;
    [SerializeField] Image exclamationImage;

    [SerializeField] float castHeight = 0.5f;
    [SerializeField] float castMinSpeed = 0.1f;
    [SerializeField] float castMaxSpeed = 3f;
    [SerializeField] float castPowMulitplier = 1.5f;

    InputActionReference fishingCastAction;
    InputActionReference cancelFishingAction;
    InputActionReference mousePositionAction;

    FishingStyle currentFishingStyle = FishingStyle.None;
    [SerializeField] FishingState currentState = FishingState.None;

    Coroutine currentCoroutine = null;

    Item baitItem = null;

    void Update()
    {
        if (UIManager.IsUIOpen) return;

        switch (currentState)
        {
            case FishingState.None:
                if (fishingCastAction.action.WasPressedThisFrame())
                    FishingCast();
                break;

            case FishingState.Casting:
                if (cancelFishingAction.action.WasPressedThisFrame())
                    FishingCancel();
                break;

            case FishingState.WaitingForBite:
                if (cancelFishingAction.action.WasPressedThisFrame() || fishingCastAction.action.WasPressedThisFrame())
                    FishingCancel();
                break;
                 
            case FishingState.Biting:
                if (fishingCastAction.action.WasPressedThisFrame())
                    PerformHook();
                if (cancelFishingAction.action.WasPressedThisFrame())
                    FishingCancel();
                break;

            case FishingState.Hooked:
                if (cancelFishingAction.action.WasPressedThisFrame())
                    FishingCancel();
                break;
        }
    }
    IEnumerator CastCoroutine(Vector3 position)
    {
        bobberTransform.position = rodTipTransform.position;
        position.z = 10f;

        //Vector3 startPos = bobberTransform.position;
        Vector3 endPos = Camera.main.ScreenToWorldPoint(position); endPos.z = 0;

        float t = 0, tt = 0;
        //float distance = Vector3.Distance(startPos, endPos);
        //float speed = Mathf.Lerp(castMinSpeed, castMaxSpeed, Mathf.InverseLerp(0f, 15f, distance));

        //Vector3 controlPoint = (startPos + endPos) / 2 + (distance * castHeight * Vector3.up);

        while (t < 1f)
        {
            //{ // 하던 것
            //    tt = Mathf.Pow(t, castPowMulitplier);
            //    Vector3 pos = (Mathf.Pow(1 - tt, 2) * startPos) + (2 * (1 - tt) * tt * controlPoint) + (Mathf.Pow(tt, 2) * endPos);
            //    bobberTransform.position = pos;
            //    t += Time.deltaTime * speed; yield return null;
            //}

            {
                t = 1;
                yield return null;
            }
        }

        bobberTransform.position = endPos;
        PickCatchItem();
    }
    void PickCatchItem()
    {
        Collider2D zoneObj = Physics2D.OverlapPoint((Vector2)bobberTransform.position, zoneLayerMask);

        if (zoneObj == null)
        {
            FishingCancel(); return;
        }

        Item bait = null;

        if (baitItem != null)
        {
            bait = baitItem;

            UseBait();
        }
        else
        {
            bait = defaultBaitItem;
        }

        currentState = FishingState.WaitingForBite;

        FishingData.SetTargetItem(
            bait.SelectItem(
                zoneObj.GetComponent<FishingZoneArea>().GetFishingZone(),
                FishingData.FishingLevel));

        currentCoroutine = StartCoroutine(WaitingCoroutine());
    }
    IEnumerator WaitingCoroutine()
    {
        float biteWaitTime = FishingData.TargetItem.GetBiteWaitTime();

        yield return new WaitForSeconds(biteWaitTime);

        currentState = FishingState.Biting;

        DOTweenHelper.Fade(exclamationImage, 0.2f, 0.6f);
        DOTweenHelper.MoveUpAnimation(exclamationImage.transform, 0.7f, 1.1f, 0.3f);

        yield return new WaitForSeconds(1f);

        FishingCancel();
    }
    void PerformHook()
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);

        currentState = FishingState.Hooked;

        currentFishingStyle = FishingData.TargetItem.GetFishingStyle();

        IFishingSystem fishingSystem = fishingSystems[currentFishingStyle].GetComponent<IFishingSystem>();

        fishingSystem.StartFishing();
    }
    void FishingCast()
    {
        GlobalStateObserver.NotifyFishingStateChanged(true);

        currentState = FishingState.Casting;
        //PlayerFishingLineController.Instance.EnableLine();

        PlayerMove.Instance.DisableMove();
        DirectionManager.Instance.DisableDirection();

        PlayerAnimator.Instance.FishingCast();
    }
    public void FishingCastEnd()
    {
        currentCoroutine = StartCoroutine(CastCoroutine(mousePositionAction.action.ReadValue<Vector2>()));
    }
    public void FishingCancel()
    {
        PlayerFishingLineController.Instance.DisableLine();

        currentState = FishingState.None;

        //if (currentFishingStyle != FishingStyle.None)
        //{
        //    IFishingSystem fishingSystem = fishingSystems[currentFishingStyle].GetComponent<IFishingSystem>();

        //    fishingSystem.CancelFishing();

        //    currentFishingStyle = FishingStyle.None;
        //}
        Debug.Log("취소 확인");
        PlayerMove.Instance.EnableMove();
        DirectionManager.Instance.EnableDirection();
        PlayerAnimator.Instance.FishingCancel();
        GlobalStateObserver.NotifyFishingStateChanged(false);

        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
    }
    public void Complete()
    {
        Debug.Log("Manager Complete");

        PlayerInventory.Instance.GetItem();

        FishingCancel();
    }
    public void SelectBait(Item item)
    {
        baitItem = item;
    }
    void UseBait()
    {
        PlayerInventory.Instance.UseBaitItem();
    }
    private void OnEnable()
    {
        fishingCastAction = InputManager.GetInputAction(InputType.FishingCast);
        mousePositionAction = InputManager.GetInputAction(InputType.MousePoint);
        cancelFishingAction = InputManager.GetInputAction(InputType.FishingCancel);
    }
    private void OnDisable()
    {
        InputManager.Release(InputType.FishingCast);
        InputManager.Release(InputType.MousePoint);
        InputManager.Release(InputType.FishingCancel);
    }
    private void Awake()
    {
        Instance = this;
    }
}
