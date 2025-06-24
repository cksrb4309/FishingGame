using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
public class UserMainController : MonoBehaviour
{
    public static UserMainController Instance { get; private set; } = null;
    public Vector2 Position => transform.position;
    [SerializeField] TMP_Text hpText;
    [SerializeField] SpriteRenderer myRenderer;
    [SerializeField] Vector2 minMoveAreaSize;
    [SerializeField] Vector2 maxMoveAreaSize;
    InputActionReference dashInputActionReference;
    InputActionReference leftInputActionReference;
    InputActionReference rightInputActionReference;
    InputActionReference upInputActionReference;
    InputActionReference downInputActionReference;
    bool isAlive = false;
    int Hp
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;

            hpText.text = hp > 0 ? hp.ToString() : "0";

            if (hp <= 0) Die();
        }
    }
    int hp;
    private void Awake()
    {
        Instance = this;
    }
    public void ReceiveDamage(int damage)
    {
        Hp -= damage;
    }
    void Die()
    {
        if (!isAlive) return;

        isAlive = false;

        Fishing_Game.fishing_Game.CancelFishing();
    }
    public void Setting()
    {
        Hp = FishingData.MiniGame_4_Data.MaxHp;

        isAlive = true;

        myRenderer.color = Color.white;
    }
    public void Cancel()
    {
        myRenderer.color = new Color(0, 0, 0, 0);
    }
    private void OnEnable()
    {
        leftInputActionReference = InputManager.GetInputAction(InputType.Left);
        rightInputActionReference = InputManager.GetInputAction(InputType.Right);
        downInputActionReference = InputManager.GetInputAction(InputType.Down);
        upInputActionReference = InputManager.GetInputAction(InputType.Up);
        dashInputActionReference = InputManager.GetInputAction(InputType.PlayerDash);
    }
    private void OnDisable()
    {
        InputManager.Release(InputType.Left);
        InputManager.Release(InputType.Right);
        InputManager.Release(InputType.Down);
        InputManager.Release(InputType.Up);
        InputManager.Release(InputType.PlayerDash);
    }


    Vector3 velocity;
    Vector3 dashVelocity;
    Vector3 nextPosition;
    Vector3 moveValue = Vector3.zero;
    void Update()
    {
        MoveInput();
        Calculate();
        ApplyMove();
    }
    void MoveInput()
    {
        moveValue.Set(0, 0, 0);

        if (leftInputActionReference.action.IsPressed())
            moveValue.x -= 1;
        if (rightInputActionReference.action.IsPressed())
            moveValue.x += 1;
        if (upInputActionReference.action.IsPressed())
            moveValue.y += 1;
        if (downInputActionReference.action.IsPressed())
            moveValue.y -= 1;
    }
    void Calculate()
    {
        
    }
    void ApplyMove()
    {
        nextPosition = transform.localPosition + velocity * Time.deltaTime;

        nextPosition.x = Mathf.Clamp(nextPosition.x, minMoveAreaSize.x, maxMoveAreaSize.x);
        nextPosition.y = Mathf.Clamp(nextPosition.y, minMoveAreaSize.y, maxMoveAreaSize.y);

        transform.localPosition = nextPosition;
    }
}
