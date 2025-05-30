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

    InputActionReference fishingCastAction;
    InputActionReference cancelFishingAction;
    InputActionReference mousePositionAction;

    FishingStyle currentFishingStyle = FishingStyle.None;
    [SerializeField] FishingState currentState = FishingState.None;

    Coroutine currentCoroutine = null;

    Item baitItem = null;
    Item targetItem = null;


    private void Update()
    {
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
    private IEnumerator CastCoroutine(Vector3 position)
    {
        bobberTransform.position = rodTipTransform.position;
        position.z = 10f;
        Vector3 startPos = bobberTransform.position;
        Vector3 endPos = Camera.main.ScreenToWorldPoint(position); endPos.z = 0;
        float distance = Vector3.Distance(startPos, endPos);
        Vector3 controlPoint = (startPos + endPos) / 2 + (distance * castHeight * Vector3.up);
        float t = 0;
        float speed = Mathf.Lerp(castMinSpeed, castMaxSpeed, Mathf.InverseLerp(0f, 15f, distance));
        //float speed = 1f;

        while (t < 1f)
        {
            Vector3 pos = (Mathf.Pow(1 - t, 2) * startPos) + (2 * (1 - t) * t * controlPoint) + (Mathf.Pow(t, 2) * endPos);

            bobberTransform.position = pos;

            t += Time.deltaTime * speed;
            yield return null;
        }

        bobberTransform.position = endPos;
        PickCatchItem();
    }
    private void PickCatchItem()
    {
        Collider2D zoneObj = Physics2D.OverlapPoint((Vector2)bobberTransform.position, zoneLayerMask);

        if (zoneObj == null)
        {
            Debug.Log("위치에 맞는 존이 없습니다");

            FishingCancel();

            return;
        }

        Item bait = null;

        if (baitItem != null)
        {
            bait = baitItem;
            // UseBait();
        }
        else
        {
            bait = defaultBaitItem;
        }

        currentState = FishingState.WaitingForBite;

        targetItem = bait.SelectItem(zoneObj.GetComponent<FishingZoneArea>().GetFishingZone());

        currentCoroutine = StartCoroutine(WaitingCoroutine());
    }
    private IEnumerator WaitingCoroutine()
    {
        float biteWaitTime = targetItem.GetBiteWaitTime();

        yield return new WaitForSeconds(biteWaitTime);

        currentState = FishingState.Biting;

        DOTweenHelper.Fade(exclamationImage, 0.2f, 0.6f);
        DOTweenHelper.MoveUpAnimation(exclamationImage.transform, 0.7f, 1.1f, 0.3f);

        yield return new WaitForSeconds(1f);

        FishingCancel();
    }
    private void PerformHook()
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);

        currentState = FishingState.Hooked;

        currentFishingStyle = targetItem.GetFishingStyle();

        IFishingSystem fishingSystem = fishingSystems[currentFishingStyle].GetComponent<IFishingSystem>();

        fishingSystem.StartFishing(targetItem);
    }
    private void FishingCast()
    {
        currentState = FishingState.Casting;
        PlayerFishingLineController.Instance.EnableLine();

        // 이동 고정, 회전 고정
        PlayerMove.Instance.DisableMove();
        DirectionManager.Instance.DisableDirection();

        currentCoroutine = StartCoroutine(CastCoroutine(mousePositionAction.action.ReadValue<Vector2>()));
    }
    private void FishingCancel()
    {
        PlayerFishingLineController.Instance.DisableLine();

        currentState = FishingState.None;

        if (currentFishingStyle != FishingStyle.None)
        {
            IFishingSystem fishingSystem = fishingSystems[currentFishingStyle].GetComponent<IFishingSystem>();

            fishingSystem.CancelFishing();

            currentFishingStyle = FishingStyle.None;
        }

        // 이동 고정, 회전 고정 해제
        PlayerMove.Instance.EnableMove();
        DirectionManager.Instance.EnableDirection();

        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
    }
    public void Complete()
    {
        FishingCancel();
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
